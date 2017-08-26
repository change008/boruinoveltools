using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tiexue.Framework.Net
{
    internal class SocketAsyncEventArgsPoolTuple
    {
        private static BufferManager _bufferPool = new BufferManager();
        private static SocketAsyncEventArgPool _socketArgsPool = new SocketAsyncEventArgPool();

        public static void InitializeAll()
        {
            _bufferPool = new BufferManager();
            _bufferPool.Initialize();

            _socketArgsPool = new SocketAsyncEventArgPool();
            _socketArgsPool.Initialize();
        }

        public static void DisposeAll()
        {
            _bufferPool.Dispose();
            _socketArgsPool.Dispose();

        }

        public static SocketAsyncEventArgPool SendArgsTuple
        { get { return _socketArgsPool; } }



        public static BufferManager SendBufferTuple
        { get { return _bufferPool; } }


    }

}
