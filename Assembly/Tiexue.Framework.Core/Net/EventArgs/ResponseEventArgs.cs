using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net 
{
    [Serializable]
    public class ResponseEventArgs : EventArgs
    {
        public PacketBase PacketContext { get; set; }
    }
}
