using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Tiexue.Framework.Net.Config;
using System.Diagnostics;
using System.Threading.Tasks;
using Tiexue.Framework.Extension;

namespace Tiexue.Framework.Net
{
    public class AsyncSocketServer : SocketServerBase
    {
        public AsyncSocketServer(IServerConfig serverConfig)
            : base(serverConfig)
        {
        }

        #region private member

        private ManualResetEvent _tcpClientConnected = new ManualResetEvent(false);

        private BufferManagerSlim m_BufferManager;

        private SocketAsyncEventArgsPoolSlim m_ReadWritePool;

        private Semaphore _maxConnectionSemaphore;

        private Socket m_ListenSocket = null;

        private Thread m_ListenThread = null;


        #endregion

        volatile List<AsyncSocketSession> _sessions = new List<AsyncSocketSession>();
        public int SessionCount { get { ClearIdleSession(new object()); return _sessions.Count; } }

        public IList<AsyncSocketSession> Sessions { get { return _sessions; } }//fake
        #region Start

        private System.Threading.Timer _checkIsBusyTimer = null;

        public override bool Start()
        {
            try
            {
                if (!base.Start())
                    return false;

                if (!InitBuffer()) return false;

                SocketAsyncEventArgsPoolTuple.InitializeAll();//!!!!!!!!!!!!!!!!

                PrepareSocketAsyncEventArgsPool();

                if (Config.ClearIdleSession)
                {
                    StartClearSessionTimer();
                }

                _checkIsBusyTimer = new Timer((new TimerCallback((t) =>
                {
                    if (ProcessorPerformanceCounters.Value.Any(n => n.NextValue() > Config.CpuUsageThreshold))
                    {
                        _isBusy = true;
                    }
                    else
                    {
                        _isBusy = false;
                    }
                })), new object(), 0, 1000);

                if (m_ListenSocket == null)
                {
                    m_ListenThread = new Thread(StartListenInternal);
                    m_ListenThread.Start();
                }

                WaitForStartComplete();
                return IsRunning;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("AsyncSocketServer Exception :", ex);
                }
                return false;
            }


        }

        private void PrepareSocketAsyncEventArgsPool()
        {
            m_ReadWritePool = new SocketAsyncEventArgsPoolSlim(Config.MaxConnectionNumber);
            // preallocate pool of SocketAsyncEventArgs objects
            SocketAsyncEventArgs socketEventArg;

            for (int i = 0; i < Config.MaxConnectionNumber; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs
                socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.UserToken = new AsyncUserToken();
                m_BufferManager.SetBuffer(socketEventArg);

                // add SocketAsyncEventArg to the pool
                m_ReadWritePool.Push(new SocketAsyncEventArgsProxy(socketEventArg));
            }
        }

        private bool InitBuffer()
        {

            int bufferSize = Math.Max(Config.ReceiveBufferSize, Config.SendBufferSize);

            if (bufferSize <= 0)
                bufferSize = 1024 * 8;

            m_BufferManager = new BufferManagerSlim(bufferSize * Config.MaxConnectionNumber * 2, bufferSize);

            try
            {
                m_BufferManager.InitBuffer();
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Warning:Not enough space ! Exception{0}", e);
                }
                return false;
            }
            return true;
        }

        private void StartListenInternal()
        {
            m_ListenSocket = new Socket(this.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                m_ListenSocket.Bind(this.EndPoint);
                m_ListenSocket.Listen(100);//Backlog this number may auto change when meet syn attack
                m_ListenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                m_ListenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Error:Server Cannot Start ! Exception {0}", ex);
                }

                OnStartCompleted();
                throw ex;
            }

            _maxConnectionSemaphore = new Semaphore(Config.MaxConnectionNumber, Config.MaxConnectionNumber);

            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArg_Completed);

            IsRunning = true;
            OnStartCompleted();

            while (!IsStopped)
            {
                _maxConnectionSemaphore.WaitOne();
                _tcpClientConnected.Reset();

                acceptEventArg.AcceptSocket = null;//reuse
                bool willRaiseEvent = true;
                try
                {
                    willRaiseEvent = m_ListenSocket.AcceptAsync(acceptEventArg);// Non block method
                }
                catch (ObjectDisposedException ex)//listener has been stopped
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketServer StartListenInternal  Exception :", ex);
                    }
                    IsRunning = false;
                    return;
                }
                catch (NullReferenceException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketServer StartListenInternal Exception :", ex);
                    }
                    IsRunning = false;
                    return;
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketServer StartListenInternal Exception :", ex);
                    }
                    IsRunning = false;
                    return;
                }

                if (!willRaiseEvent)
                    AcceptNewClient(acceptEventArg);

                _tcpClientConnected.WaitOne();
            }

            IsRunning = false;
        }


        #endregion

        private void acceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            AcceptNewClient(e);
        }

        private void AcceptNewClient(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                //Get the socket for the accepted client connection and put it into the 
                //ReadEventArg object user token
                SocketAsyncEventArgsProxy socketEventArgsProxy = m_ReadWritePool.Pop();
                socketEventArgsProxy.Socket = e.AcceptSocket;
                if (Config.Debug)
                {
                    Console.Write("-");
                }
                //Console.WriteLine("Client {0} now in .", e.AcceptSocket.RemoteEndPoint.ToString());
                //  Console.WriteLine("Now {0}", _sessions.Count);
                AsyncSocketSession session = RegisterSession<AsyncSocketSession>(e.AcceptSocket);
                session.SocketAsyncProxy = socketEventArgsProxy;
                session.Config = Config;
                //Notice here :订阅了事件 
                //在会话结束的时候要执行取消会话的操作 否则由还存在调用关系导致GC无法正常进行
                session.Closed += new EventHandler<SocketSessionClosedEventArgs>(session_Closed);
                session.OnRequestReceived += new EventHandler<SocketPlainTextEventArgs>(OnRequestReceived);

                _tcpClientConnected.Set();
                session.Start();

                _sessions.Add(session);// Demo 
            }
            else
            {
                _tcpClientConnected.Set();
            }
        }



        private Lazy<PerformanceCounter[]> ProcessorPerformanceCounters = new Lazy<PerformanceCounter[]>(() =>
        {
            PerformanceCounter[] counters = new PerformanceCounter[System.Environment.ProcessorCount];
            for (int i = 0; i < counters.Length; i++)
            {
                counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            return counters;
        });

        private bool _isBusy = false;
        public void Broadcast(SocketPlainTextEventArgs e)
        {
            if (_isBusy || _sessions.Count == 0)
            {
                return;
            }

            try
            {
                Byte[] messageBuffer = Encoding.UTF8.GetBytes(e.Content);
                var temp = e.Condition == null ? _sessions.Where(n => n != null) : _sessions.Where(e.Condition);
                temp.AsParallel().ForAll(n =>
                {
                    if (n != null)
                    {
                        n.SendAsyc(messageBuffer);
                    }
                });
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("AsyncSocketServer Broadcast Exception:", ex);
                }
            }
        }


        public void OnRequestReceived(object sender, SocketPlainTextEventArgs e)
        {
            Broadcast(e);
        }

        private void session_Closed(object sender, SocketSessionClosedEventArgs e)
        {
            try
            {
                _maxConnectionSemaphore.Release();
                AsyncSocketSessionBase socketSession = sender as AsyncSocketSessionBase;
                socketSession.Closed -= new EventHandler<SocketSessionClosedEventArgs>(session_Closed);
                socketSession.OnRequestReceived -= new EventHandler<SocketPlainTextEventArgs>(OnRequestReceived);

                if (socketSession != null && this.m_ReadWritePool != null)
                    this.m_ReadWritePool.Push(socketSession.SocketAsyncProxy);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("AsyncSocketServer session_Closed Exception:", ex);
                }
            }
        }

        #region Stop Dispose
        public override void Stop()
        {
            base.Stop();

            if (m_ListenSocket != null)
            {
                m_ListenSocket.Close();
                m_ListenSocket = null;
            }

            if (m_ReadWritePool != null)
                m_ReadWritePool = null;

            if (m_BufferManager != null)
                m_BufferManager = null;

            VerifySocketServerRunning(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsRunning)
                    Stop();

                _tcpClientConnected.Close();
                _maxConnectionSemaphore.Close();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ClearIdleSession

        private System.Threading.Timer _clearIdleSessionTimer = null;

        private void StartClearSessionTimer()
        {
            int interval = Config.ClearIdleSessionInterval * 1000;//in milliseconds
            _clearIdleSessionTimer = new System.Threading.Timer(ClearIdleSession, new object(), interval, interval);

        }
        private object m_SessionSyncRoot = new object();
        private void ClearIdleSession(object state)
        {
            if (Monitor.TryEnter(state))
            {
                try
                {
                    lock (_sessions)
                    {

                        _sessions.Where(item => DateTime.Now.Subtract(item.LastActiveTime).TotalMinutes > Config.IdleSessionTimeOut).ToList().ForEach(session => session.Close());
                        _sessions.RemoveAll(item => (item.IsClosed || DateTime.Now.Subtract(item.LastActiveTime).TotalMinutes > Config.IdleSessionTimeOut));

                        Console.WriteLine("Current Session Count:{0}", _sessions.Count);

                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketServer ClearIdleSession Exception:", ex);
                    }
                }
            }
        }

        #endregion
    }
}
