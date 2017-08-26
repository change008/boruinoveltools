using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Tiexue.Framework.Extension;
using System.Collections;

namespace Tiexue.Framework.Net
{
    abstract class AsyncCommandResolver : IAsyncCommandResolver
    {
        #region log4net
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger("TiexueFrameworkCore.Net");
        #endregion
        private List<Byte> receiveBuffer;
        private readonly byte[] _policyRequest = Encoding.UTF8.GetBytes("<policy-file-request/>\0");
        private StreamBuffer _streamBuffer;
        public AsyncCommandResolver()
        {
            _streamBuffer = new StreamBuffer();
            //receiveBuffer = new List<byte>();
        }

        public AsyncCommandResolver(IAsyncCommandResolver prevReader)
        {
            _streamBuffer = prevReader.Buffer;
            //  receiveBuffer = prevReader.CommandBuffer;
        }

        #region IAsyncCommandReader Members

        public abstract ResolveResult Parse(SocketAsyncEventArgs e, byte endMark);

        public List<byte> CommandBuffer
        {
            get { return receiveBuffer; }
        }

        public StreamBuffer Buffer
        {
            get { return _streamBuffer; }
        }

        #endregion

        protected void SaveBuffer2(byte[] newData, int offset, int length)
        {
            _streamBuffer.Put(newData, offset, length);
        }

        protected byte[] SaveBuffer(byte[] newData, int offset, int length)
        {
            _streamBuffer.Put(newData, offset, length);

            return _streamBuffer.ToByteArrays();

        }




        private List<string> commands = new List<string>();
        protected ResolveResult FindCommandDirectly(SocketAsyncEventArgs e, int offset, byte endMark)
        {
            var results = e.Buffer.IndexesOf(offset, e.BytesTransferred, endMark).ToArray<int>();
            try
            {
                if (results != null && results.Length > 0)
                {
                    commands.Add(Encoding.UTF8.GetString(SaveBuffer(e.Buffer, e.Offset, results[0] - e.Offset)));
                    _streamBuffer.Initialize();
                    //receiveBuffer.Clear();

                    for (int i = 1; i < results.Length; i++)
                    {
                        commands.Add(Encoding.UTF8.GetString(e.Buffer, results[i - 1] + 1, results[i] - results[i - 1] - 1));
                    }

                    int last = results.LastOrDefault() + 1;
                    int length = (e.BytesTransferred - last + e.Offset - 1);
                    if (length > 0)
                    {
                        SaveBuffer(e.Buffer, last, length);
                    }
                }
                else
                {
                    SaveBuffer2(e.Buffer, e.Offset, e.BytesTransferred);
                    if (_policyRequest.Length == 23)
                    {
                        if (_policyRequest.SequenceEqual(_streamBuffer.ToByteArrays()))
                        {
                            commands.Add("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0");
                            return new ResolveResult() { Requests = commands, FoundCompleteRequest = true, RequestType = RequestType.PolicyFileRequest };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("FindCommandDirectly session_Closed Exception:", ex);
                }
            }
            return new ResolveResult() { Requests = commands, FoundCompleteRequest = (results.Length > 0), RequestType = RequestType.CommonText };
        }






    }
}
