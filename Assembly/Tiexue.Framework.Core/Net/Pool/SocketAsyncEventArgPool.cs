using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Threading;
using System.Net.Sockets;

namespace Tiexue.Framework.Net
{

    internal class SocketAsyncEventArgPool : IDisposable
    {
        #region Fields

        private object _syncRoot = new object();
        private int _capacity = 5000;

        #endregion

        #region Constructor Method

        public SocketAsyncEventArgPool()
        {
            _capacity = 5000;//use config later
        }

        public void Initialize()
        {
            GCLatencyMode mode = GCSettings.LatencyMode;
            GCSettings.LatencyMode = GCLatencyMode.Batch;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


            ArgsPool = new Stack<SocketAsyncEventArgs>();
            Overlapped[] overlapped = new Overlapped[_capacity];

            for (int i = 0; i < _capacity; i++)
            {
                overlapped[i] = new Overlapped();
                Push(new SocketAsyncEventArgs());
            }
            GC.KeepAlive(overlapped);
            overlapped = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            GCSettings.LatencyMode = mode;
        }

        #endregion

        #region Methods

        internal void Push(SocketAsyncEventArgs item)
        {
            lock (_syncRoot) ArgsPool.Push(item);
        }

        internal SocketAsyncEventArgs Pop()
        {
            lock (_syncRoot)
                return ArgsPool.Count > 0 ? ArgsPool.Pop() : (new SocketAsyncEventArgs());
        }

        #endregion

        #region IDisposable Member

        ~SocketAsyncEventArgPool()
        {
            Dispose(false);
        }

        private bool _disposed = false;

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
                    foreach (SocketAsyncEventArgs args in ArgsPool)
                        args.Dispose();
                    ArgsPool.Clear();
                }
                _disposed = true;
            }
        }

        #endregion

        #region Attribute

        private Stack<SocketAsyncEventArgs> ArgsPool
        { get; set; }


        #endregion
    }
}
