using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    public class SocketSessionClosedEventArgs : EventArgs
    {
        public string SessionID { get; set; }
    }
}
