using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net 
{
    public sealed class Request : PacketBase
    {
        #region Methods

        public Request()
        { }

        public Request(IPacket packet)
        {
            Header = packet.Header.Value;
            Command = packet.Command.Value;
            From = packet.From.Value;
            To = packet.To.Value;
            CallID = packet.CallID.Value;
            Body = packet.Body.Value;
            Tail = packet.Tail.Value;
        }

        #endregion
    }
}
