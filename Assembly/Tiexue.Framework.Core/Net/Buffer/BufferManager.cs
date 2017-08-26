using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Net.Sockets;
using System.Threading;
using Tiexue.Framework.Net.Config;

namespace Tiexue.Framework.Net
{
    internal class BufferManager : IDisposable
    {
        #region Fields

        private object _syncRoot = new object();
        private Stack<int> _freeIndexPool;

        private int _currentIndex;
        private int _numberOfBuffers = 5000;
        private int _bufferSize = 128;
        private int _totalBytes;

        private byte[] _buffer;

        #endregion

        #region  Methods

        public BufferManager()
        {
        }

        public void Initialize()
        {
            _numberOfBuffers = 5000;//stub using config later
            GCLatencyMode mode = GCSettings.LatencyMode;
            GCSettings.LatencyMode = GCLatencyMode.Batch;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            _freeIndexPool = new Stack<int>();
            _totalBytes = _numberOfBuffers * _bufferSize;
            _buffer = new byte[_totalBytes];
            _currentIndex = 0;


            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            GCSettings.LatencyMode = mode;
        }

        internal void Pop(SocketAsyncEventArgs args)
        {
            lock (_syncRoot)
            {

                if (_freeIndexPool.Count > 0)
                {
                    //clear the _buffer before checkout!
                    int offset = _freeIndexPool.Pop();
                    args.SetBuffer(_buffer, offset, _bufferSize);
                }
                else
                {
                    args.SetBuffer(_buffer, _currentIndex, _bufferSize);
                    Interlocked.Add(ref _currentIndex, _bufferSize);
                }
            }

        }

        internal void Push(SocketAsyncEventArgs args)
        {
            lock (_syncRoot)
            {
                _freeIndexPool.Push(args.Offset);
            }
        }

        #endregion

        #region Attribute

        internal int AvalidableBufferPoolCount
        {
            get { return ((_totalBytes - _currentIndex) / _bufferSize + _freeIndexPool.Count); }
        }

        #endregion

        #region IDisposable Members

        private Boolean _disposed = false;
        ~BufferManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _freeIndexPool.Clear();
                    _buffer = null;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
