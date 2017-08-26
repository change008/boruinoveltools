using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net 
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class NetworkPerformanceCounterCategoryAttribute : Attribute
    {
        public string Name
        { get; private set; }

        public NetworkPerformanceCounterCategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
