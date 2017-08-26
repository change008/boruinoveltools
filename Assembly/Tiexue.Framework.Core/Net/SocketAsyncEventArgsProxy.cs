using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Tiexue.Framework.Net
{
    public class SocketAsyncEventArgsProxy
    {
        public SocketAsyncEventArgs SocketEventArgs { get; private set; }

        private SocketAsyncEventArgsProxy()
        {
        }


        public SocketAsyncEventArgsProxy(SocketAsyncEventArgs socketEventArgs)
        {
            SocketEventArgs = socketEventArgs;
            SocketEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArgs_Completed);
        }

        public static void SocketEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {

            var token = e.UserToken as AsyncUserToken;
            var socketSession = token.SocketSession;

            if (socketSession == null)
                return;

            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    socketSession.ProcessReceived(e);
                    break;
                case SocketAsyncOperation.Send:
                    socketSession.ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }

        public Socket Socket
        {
            set
            {
                ((AsyncUserToken)SocketEventArgs.UserToken).Socket = value;
            }
        }

        public void Initialize(Socket socket, AsyncSocketSessionBase socketSession)
        {
            var token = SocketEventArgs.UserToken as AsyncUserToken;
            token.Socket = socket;
            token.SocketSession = socketSession;

        }

        public void Reset()
        {
            var token = SocketEventArgs.UserToken as AsyncUserToken;
            token.Socket = null;
            token.SocketSession = null;
        }
    }
}
