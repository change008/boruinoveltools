using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net.Protocol
{
    public sealed class Packet : IPacket
    {
    

        #region Attributes

        public Header Header
        { get; private set; }

        public FromChars From
        { get; private set; }

        public ToChars To
        { get; private set; }

        public Command Command
        { get; private set; }

        public CallID CallID
        { get; private set; }

        public Length Length
        { get; private set; }

        public Body Body
        { get; set; }

        public Tail Tail
        { get; private set; }

        #endregion

        #region Constructor

        public Packet()
        {
            Header = new Header();
            Command = new Command();
            From = new FromChars();
            To = new ToChars();
            CallID = new CallID();
            Length = new Length();
            Tail = new Tail();
        }

        #endregion

        #region Read Method

        public bool Read(StreamBuffer stream)
        {
            try
            {
                Header.Parse(stream);
                if (!Header.Value.SequenceEqual(Global.TOAD_HEADER)) return false;

                Command.Parse(stream);
                From.Parse(stream);
                To.Parse(stream);
                CallID.Parse(stream);
                Length.Parse(stream);
                Body = new Body(Length.Value);
                Body.Parse(stream);
                Tail.Parse(stream);
                return true;
            }
            catch (Exception ex)
            {
              //  _debug.Error(ex, "Packet read");
                return false;
            }
        }

        #endregion
    }
}
