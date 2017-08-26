using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tiexue.Framework.Net.Config
{
    public interface IServerConfig
    {
        string IP { get; }

        int Port { get; }

        bool Disabled { get; }

        string Name { get; }

        bool Debug { get; }

        int MaxConnectionNumber { get; }

        int ReceiveBufferSize { get; }

        int SendBufferSize { get; }

        int CpuUsageThreshold { get; }

        int GroupCount { get; }
        /// <summary>
        /// Gets a value indicating whether clear idle session.
        /// </summary>
        /// <value><c>true</c> if clear idle session; otherwise, <c>false</c>.</value>
        bool ClearIdleSession { get; }

        /// <summary>
        /// Gets the clear idle session interval, in seconds.
        /// </summary>
        /// <value>The clear idle session interval.</value>
        int ClearIdleSessionInterval { get; }


        /// <summary>
        /// Gets the idle session timeout time length, in minutes.
        /// </summary>
        /// <value>The idle session time out.</value>
        int IdleSessionTimeOut { get; }

    }
}
