using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tiexue.Framework.Net.Config
{
    public class ServerConfig : ConfigurationElement, IServerConfig
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
        }

        [ConfigurationProperty("ip", IsRequired = false)]
        public string IP
        {
            get { return this["ip"] as string; }

        }


        [ConfigurationProperty("debug", IsRequired = true)]
        public bool Debug
        {
            get { return (bool)this["debug"]; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
        }

        [ConfigurationProperty("groupcount", IsRequired = true, DefaultValue = 15)]
        public int GroupCount
        {
            get { return (int)this["groupcount"]; }
        }

        [ConfigurationProperty("cputhreshold", IsRequired = true)]
        public int CpuUsageThreshold
        {
            get { return (int)this["cputhreshold"]; }
        }

        [ConfigurationProperty("mode", IsRequired = false, DefaultValue = "Async")]
        public SocketMode Mode
        {
            get { return (SocketMode)this["mode"]; }
        }

        [ConfigurationProperty("disabled", DefaultValue = "false")]
        public bool Disabled
        {
            get { return (bool)this["disabled"]; }
        }

        [ConfigurationProperty("maxConnectionNumber", IsRequired = false, DefaultValue = 10000)]
        public int MaxConnectionNumber
        {
            get { return (int)this["maxConnectionNumber"]; }
        }

        [ConfigurationProperty("receiveBufferSize", IsRequired = false, DefaultValue = 256)]
        public int ReceiveBufferSize
        {
            get { return (int)this["receiveBufferSize"]; }
        }

        [ConfigurationProperty("sendBufferSize", IsRequired = false, DefaultValue = 256)]
        public int SendBufferSize
        {
            get { return (int)this["sendBufferSize"]; }
        }



        /// <summary>
        /// Gets a value indicating whether clear idle session.
        /// </summary>
        /// <value><c>true</c> if clear idle session; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("clearIdleSession", IsRequired = false, DefaultValue = false)]
        public bool ClearIdleSession
        {
            get { return (bool)this["clearIdleSession"]; }
        }

        /// <summary>
        /// Gets the clear idle session interval, in seconds.
        /// </summary>
        /// <value>The clear idle session interval.</value>
        [ConfigurationProperty("clearIdleSessionInterval", IsRequired = false, DefaultValue = 120)]
        public int ClearIdleSessionInterval
        {
            get { return (int)this["clearIdleSessionInterval"]; }
        }


        /// <summary>
        /// Gets the idle session timeout time length, in minutes.
        /// </summary>
        /// <value>The idle session time out.</value>
        [ConfigurationProperty("idleSessionTimeOut", IsRequired = false, DefaultValue = 20)]
        public int IdleSessionTimeOut
        {
            get { return (int)this["idleSessionTimeOut"]; }
        }


    }
}
