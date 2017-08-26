using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    public interface IPacket
    {
        Header Header { get; }
        FromChars From { get; }
        ToChars To { get; }
        Command Command { get; }
        CallID CallID { get; }
        Length Length { get; }
        Body Body { get; set; }
        Tail Tail { get; }
        bool Read(StreamBuffer stream);
    }
}
