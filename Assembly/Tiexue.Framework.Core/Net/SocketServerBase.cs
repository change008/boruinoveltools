using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Tiexue.Framework.Net.Config;
using Tiexue.Framework.Extension;

namespace Tiexue.Framework.Net
{
    public abstract class SocketServerBase : IDisposable
    {
        #region log4net
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger("TiexueFrameworkCore.Net");
        #endregion

        protected object SyncRoot = new object();

        public IServerConfig Config { get; private set; }

        public bool IsRunning { get; protected set; }

        protected bool IsStopped { get; set; }


        public IPEndPoint EndPoint
        {
            get;
            private set;
        }

        private ManualResetEvent m_ServerStartupEvent = new ManualResetEvent(false);

        public SocketServerBase(IServerConfig config)
        {
            Config = config;
            EndPoint =  new IPEndPoint((string.IsNullOrEmpty(config.IP) ? IPAddress.Any : IPAddress.Parse(config.IP)), config.Port);
        }

        public virtual bool Start()
        {
            IsStopped = false;
            m_ServerStartupEvent.Reset();
            return true;
        }

        protected void WaitForStartComplete()
        {
            m_ServerStartupEvent.WaitOne();
        }

        protected void OnStartCompleted()
        {
            m_ServerStartupEvent.Set();
        }

        protected virtual T RegisterSession<T>(Socket client) where T : AsyncSocketSessionBase, new()
        {
            //load socket setting
            if (Config.ReceiveBufferSize > 0)
                client.ReceiveBufferSize = Config.ReceiveBufferSize;

            if (Config.SendBufferSize > 0)
                client.SendBufferSize = Config.SendBufferSize;

            T session = new T();
            session.UserID = QuickRandom.Instance.Next(1, 1500);
            session.GroupID = QuickRandom.Instance.Next(1, Config.GroupCount);
            session.Initialize(client);

            return session;
        }

        protected bool VerifySocketServerRunning(bool isRunning)
        {
            //waiting 10 seconds
            int steps = 10000;

            while (steps > 0)
            {
                Thread.Sleep(100);

                if (IsRunning == isRunning)
                    return true;

                steps--;
            }

            return false;
        }

        public virtual void Stop()
        {
            IsStopped = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                m_ServerStartupEvent.Close();
        }

        #endregion
    }
}
