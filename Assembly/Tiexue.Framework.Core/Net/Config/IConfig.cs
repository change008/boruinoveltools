using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Net.Config
{
    public interface IAppConfig
    {
        List<IServerConfig> GetServerList();
        string ConsoleBaseAddress { get; }
    }
}
