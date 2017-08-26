using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tiexue.Framework.Net
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NetworkPerformanceCounterAttribute : Attribute
    {
        #region Attributes

        public string Help
        { get; private set; }

        public string Name
        { get; private set; }

        public PerformanceCounterType Type
        { get; private set; }

        #endregion

        public NetworkPerformanceCounterAttribute(string name, string help, PerformanceCounterType type)
        {
            Name = name;
            Help = help;
            Type = type;
        }
    }
}
