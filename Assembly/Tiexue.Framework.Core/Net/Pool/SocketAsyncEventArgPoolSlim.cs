using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    /// <summary>
    /// Represents a collection of resusable SocketAsyncEventArgs objects.  
    /// </summary>
    class SocketAsyncEventArgsPoolSlim
    {
        Stack<SocketAsyncEventArgsProxy> m_pool;

        /// <summary>
        /// Initializes the object pool to the specified size
        /// </summary>
        /// <param name="capacity">The maximum number of SocketAsyncEven tArgs objects the pool can hold</param>
        public SocketAsyncEventArgsPoolSlim(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgsProxy>(capacity);
        }

        /// <summary>
        /// Add a SocketAsyncEventArg instance to the pool
        /// </summary>
        /// <param name="item">The SocketAsyncEventArgs instance to add to the pool</param>
        public void Push(SocketAsyncEventArgsProxy item)
        {
            if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (m_pool)
            {
                item.Socket = null;
                m_pool.Push(item);
            }
        }

        /// <summary>
        /// Removes a SocketAsyncEventArgs instance from the pool
        /// </summary>
        /// <returns>The object removed from the pool</returns>
        public SocketAsyncEventArgsProxy Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        /// <summary>
        /// The number of SocketAsyncEventArgs instances in the pool
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }

    }
}
