using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Tiexue.Framework.Net.Config;
using Tiexue.Framework.Extension;
using System.Collections;

namespace Tiexue.Framework.Net
{
    public abstract class AsyncSocketSessionBase
    {
        #region log4net
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger("TiexueFrameworkCore.Net");
        #endregion

        public abstract void Start();

        public abstract void ProcessReceived(SocketAsyncEventArgs e);

        public abstract void ProcessSend(SocketAsyncEventArgs e);

        public IServerConfig Config { get; set; }

        protected readonly object SyncRoot = new object();

        public void Initialize(Socket client)
        {
            Client = client;
        }



        #region Properties

        public Hashtable Context { get; set; }

        public SocketAsyncEventArgsProxy SocketAsyncProxy
        {
            get;
            set;
        }

        /// <summary>
        /// Fake
        /// </summary>
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Fake
        /// </summary>
        public int GroupID
        {
            get;
            set;
        }
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public Socket Client { get; set; }

        private bool m_IsClosed = false;

        public bool IsClosed
        {
            get { return m_IsClosed; }
        }
        /// <summary>
        /// The session identity string
        /// </summary>
        private string m_SessionID = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        /// <value>The session ID.</value>
        public string SessionID
        {
            get { return m_SessionID; }
        }

        private DateTime m_StartTime = DateTime.Now;
        /// <summary>
        /// Gets the session start time.
        /// </summary>
        /// <value>The session start time.</value>
        public DateTime StartTime
        {
            get { return m_StartTime; }
        }

        private DateTime m_LastActiveTime = DateTime.Now;

        /// <summary>
        /// Gets the last active time of the session.
        /// </summary>
        /// <value>The last active time.</value>
        public DateTime LastActiveTime
        {
            get { return m_LastActiveTime; }
            protected set { m_LastActiveTime = value; }
        }



        #endregion

        #region EventHandler

        public event EventHandler<SocketPlainTextEventArgs> OnRequestReceived;

        public event EventHandler<StreamEventArgs> OnRequestStreamReceived;


        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler<SocketSessionClosedEventArgs> Closed;
        /// <summary>
        /// Called when [close].
        /// </summary>
        protected virtual void OnClose()
        {
            if (Closed != null)
                Closed(this, new SocketSessionClosedEventArgs { SessionID = this.SessionID });
        }

        #endregion

        /// <summary>
        /// Closes this connection.
        /// </summary>
        public virtual void Close()
        {
            if (Client == null && m_IsClosed)
                return;

            lock (SyncRoot)
            {
                if (Client == null && m_IsClosed)
                    return;
                try
                {
                    Client.Shutdown(SocketShutdown.Both);
                    if (Config.Debug)
                    {
                        Console.Write("0");
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketSessionBase ProcessReceive Exception:", ex);
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketSessionBase ProcessReceive Exception:", ex);
                    }
                }

                try
                {
                    if (Client != null)
                        Client.Close();
                }
                catch (ObjectDisposedException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketSessionBase ProcessReceive Exception:", ex);
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("AsyncSocketSessionBase ProcessReceive Exception:", ex);
                    }
                }
                finally
                {
                    Client = null;
                    m_IsClosed = true;
                    OnClose();
                }
            }
        }


    }

}
