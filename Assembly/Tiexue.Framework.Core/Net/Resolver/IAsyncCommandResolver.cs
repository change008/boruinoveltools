using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Tiexue.Framework.Net
{
    interface IAsyncCommandResolver
    {
        ResolveResult Parse(SocketAsyncEventArgs e, byte endMark);
        List<Byte> CommandBuffer { get; }
        StreamBuffer Buffer { get; }


    }
}
