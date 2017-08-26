using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tiexue.Framework.Net
{
    internal enum RequestType
    {
        PolicyFileRequest,
        CommonText,
        Command
    }

    internal class ResolveResult
    {
        public bool FoundCompleteRequest { get; set; }
        public IList<string> Requests { get; set; }
        public RequestType RequestType { get; set; }
    }
}
