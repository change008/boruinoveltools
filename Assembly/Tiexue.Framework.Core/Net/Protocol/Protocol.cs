using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net
{
    [Serializable]
    public sealed class Header : Atomic<byte[]>
    {
        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetByteArray(Global.TOAD_HEADER_LENGTH);
        }

    }


    [Serializable]
    public sealed class Body : Atomic<byte[]>
    {
        private uint Len { get; set; }

        public Body(uint length)
        {
            Len = length;
        }

        public override void Parse(StreamBuffer buf)
        {
            if (Len > Global.TOAD_BODY_LENGTH || Len < 0)
            {
                throw new OverflowException("Body length out Max:" + Global.TOAD_BODY_LENGTH);
            }
            Value = buf.GetByteArray(Len);
        }

    }

    [Serializable]
    public sealed class CallID : Atomic<long>
    {
        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetLong();
        }
    }

    [Serializable]
    public sealed class Length : Atomic<uint>
    {
        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetUInt();
            if (Value > uint.MaxValue)
                throw new OverflowException("Body len overflow:" + Value);
        }
    }

    [Serializable]
    public sealed class Command : Atomic<ushort>
    {

        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetUShort();
        }

    }

    [Serializable]
    public sealed class Tail : Atomic<byte[]>
    {

        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetByteArray(Global.TOAD_TAIL_LENGTH);
        }
    }

    [Serializable]
    public sealed class FromChars : Atomic<byte[]>
    {
        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetByteArray(Global.TOAD_TOADIDCHARS_LENGTH);
        }
    }

    [Serializable]
    public sealed class ToChars : Atomic<byte[]>
    {
        public override void Parse(StreamBuffer buf)
        {
            Value = buf.GetByteArray(Global.TOAD_TOADIDCHARS_LENGTH);
        }
    }

}
