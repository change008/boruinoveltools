using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tiexue.Framework.Net.Config
{
    public class AsyncServerConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("servers")]
        public ServerCollection Servers
        {
            get
            {
                return this["servers"] as ServerCollection;
            }
        }
    }
}
