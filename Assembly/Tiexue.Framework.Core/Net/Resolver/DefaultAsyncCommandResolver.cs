using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Tiexue.Framework.Net
{
      class DefaultAsyncCommandResolver : AsyncCommandResolver
    {
        public DefaultAsyncCommandResolver()
            : base()
        {

        }

        public DefaultAsyncCommandResolver(IAsyncCommandResolver prevReader)
            : base(prevReader)
        {

        }

        public override ResolveResult Parse(SocketAsyncEventArgs e, byte endMark)
        {
            return FindCommandDirectly(e, e.Offset, endMark);
        }
    }
}
