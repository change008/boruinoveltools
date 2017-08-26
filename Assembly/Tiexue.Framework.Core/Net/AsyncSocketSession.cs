using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Tiexue.Framework.Extension;

namespace Tiexue.Framework.Net
{
    public class AsyncSocketSession : AsyncSocketSessionBase
    {
        public event EventHandler<SocketPlainTextEventArgs> OnRequestReceived;

        public event EventHandler<StreamEventArgs> OnRequestStreamReceived;

        public override void Start()
        {
            Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            SocketAsyncProxy.Initialize(Client, this);
            ContinueToListen(SocketAsyncProxy.SocketEventArgs);
        }

        public override void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Send)
                return;

            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // read the next block of data send from the client
                if (!token.Socket.ReceiveAsync(e))
                {
                    ProcessReceived(e);
                }
            }
            else
            {
                Close();
            }
        }

        public void Send(PacketBase packet)
        {
            SendAsyc(packet.AllBuffers);
        }

        private object _sendRoot = new object();

        public void SendAsyc(byte[] buffer)
        {
            SocketAsyncEventArgs _sendArgs = null;

            try
            {
                if (Client == null) return;
                _sendArgs = SocketAsyncEventArgsPoolTuple.SendArgsTuple.Pop();
                SocketAsyncEventArgsPoolTuple.SendBufferTuple.Pop(_sendArgs);

                lock (_sendRoot)
                {
                    //_sendArgs.SetBuffer(buffer, 0, buffer.Length);
                    Buffer.BlockCopy(buffer, 0, _sendArgs.Buffer, _sendArgs.Offset, buffer.Length);
                    if (Client.Connected)
                    {
                        Client.SendAsync(_sendArgs);
                        // Array.Clear(_sendArgs.Buffer, _sendArgs.Offset, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Session SendAsyc Exception:", ex);
                }
            }
            finally
            {
                if (_sendArgs != null)
                {
                    SocketAsyncEventArgsPoolTuple.SendBufferTuple.Push(_sendArgs);
                    _sendArgs.Dispose();
                }
                _sendArgs = new SocketAsyncEventArgs();
                SocketAsyncEventArgsPoolTuple.SendArgsTuple.Push(_sendArgs);
            }
        }

        private object _receivedRoot = new object();
        private PacketParser _packetParser;

        // public override void ProcessReceived(SocketAsyncEventArgs args)
        public void ProcessReceived2010(SocketAsyncEventArgs args)
        {
            int offset = 0;

            lock (_receivedRoot)
            {
                int transLen = args.BytesTransferred;
                if (transLen == 0 || args.SocketError != SocketError.Success)
                {
                    Close(); return;
                }

                try
                {
                    offset = args.Offset;
                    Byte[] data = new Byte[transLen];
                    Buffer.BlockCopy(args.Buffer, offset, data, 0, data.Length);

                    if (Client.Connected)
                    {
                        _packetParser.CheckPacket(data, (packetContext) =>
                        {
                            OnRequestStreamReceived.OnEvent<StreamEventArgs>(this, new StreamEventArgs(this, packetContext));
                        });

                        Array.Clear(args.Buffer, args.Offset, transLen);
                        ContinueToListen(args);
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("ProcessReceived Exception:", ex);
                    }
                }
            }
        }

        private void ContinueToListen(SocketAsyncEventArgs args)
        {
            if (IsClosed)
                return;
            try
            {
                if (Client.Connected)
                {
                    bool asyncReceive = false;
                    using (ExecutionContext.SuppressFlow()) asyncReceive = Client.ReceiveAsync(this.SocketAsyncProxy.SocketEventArgs);

                    if (!asyncReceive)
                        ReceivedCompleted(this, this.SocketAsyncProxy.SocketEventArgs);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("ContinueToListen Exception:", ex);
                }
                Close();
                return;
            }
        }

        private void ReceivedCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceived(args);
            }
        }

        IAsyncCommandResolver _commandResolver = new DefaultAsyncCommandResolver();

        public override void ProcessReceived(SocketAsyncEventArgs e)
        {
            // check if the remote host closed the connection
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
            {
                Close();
                return;
            }

            this.LastActiveTime = DateTime.Now;
            Console.Write(".");
            ResolveResult result = _commandResolver.Parse(e, Encoding.UTF8.GetBytes("|")[0]);

            if (result.FoundCompleteRequest)
            {
                _commandResolver = new DefaultAsyncCommandResolver(_commandResolver);
                string commandLine = result.RequestType == RequestType.PolicyFileRequest ? result.Requests[0] : string.Join<string>("|", result.Requests);

                if (string.IsNullOrEmpty(commandLine))
                {
                    if (!token.Socket.ReceiveAsync(e))
                    {
                        ProcessReceived(e);
                    }
                    return;
                }

                try
                {
                    byte[] response = Encoding.UTF8.GetBytes(commandLine);//Take Care ！！！
                    Buffer.BlockCopy(response, 0, e.Buffer, e.Offset, response.Length);
                    e.SetBuffer(e.Offset, response.Length);//CPU high here
                    bool willRaiseEvent = token.Socket.SendAsync(e);
                    if (!willRaiseEvent)
                    {
                        ProcessSend(e);
                    }

                    if (result.RequestType != RequestType.PolicyFileRequest)
                    {
                        OnRequestReceived.OnEvent<SocketPlainTextEventArgs>(this, new SocketPlainTextEventArgs() { Content = commandLine });
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("Session ProcessReceive Exception:", ex);
                    }
                }
            }
            else
            {
                _commandResolver = new DefaultAsyncCommandResolver(_commandResolver);
                if (!token.Socket.ReceiveAsync(e))
                {
                    ProcessReceived(e);
                }
                return;
            }
        }
    }
}