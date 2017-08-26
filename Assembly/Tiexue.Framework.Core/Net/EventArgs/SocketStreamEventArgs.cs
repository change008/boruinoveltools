using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    [Serializable]
    public class StreamEventArgs : EventArgs
    {
        public StreamEventArgs(AsyncSocketSessionBase session, PacketBase packetContext)
        {
            CurrentSession = session;
            PacketContext = packetContext;
        }

        public AsyncSocketSessionBase CurrentSession { get; set; }
        public PacketBase PacketContext { get; set; }
    }

}
