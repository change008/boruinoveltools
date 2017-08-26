using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    public class SocketPlainTextEventArgs : EventArgs
    {
        public string Content { get; set; }
        public Func<AsyncSocketSession, bool> Condition { get; set; }
    }
}
