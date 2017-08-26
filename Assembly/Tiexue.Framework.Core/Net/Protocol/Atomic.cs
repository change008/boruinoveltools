using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net 
{
    public abstract class Atomic<T>
    {
        public abstract void Parse(StreamBuffer buf);
        public virtual T Value { get; set; }
        protected void PopulateBody(StreamBuffer buf) { }
    }
}
