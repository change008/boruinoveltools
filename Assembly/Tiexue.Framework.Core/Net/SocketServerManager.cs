using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiexue.Framework.Net.Config;

namespace Tiexue.Framework.Net
{
    public static class SocketServerManager
    {
        private static List<SocketServerBase> m_ServerList = new List<SocketServerBase>();

        private static Dictionary<string, Type> m_ServiceDict = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        private static IAppConfig m_Config;

        /// <summary>
        /// Initializes with the specified config.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static bool Initialize(IAppConfig config)
        {
            return true;
        }

        /// <summary>
        /// Starts with specified config.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static bool Start(IAppConfig config)
        {
            List<IServerConfig> serverList = config.GetServerList();

            Type serviceType = null;

            //ServiceCredentials credentials = null;

            //if (config.CredentialConfig != null)
            //    credentials = GetServiceCredentials(config.CredentialConfig);

            //foreach (IServerConfig serverConfig in serverList)
            //{
            //    if (serverConfig.Disabled)
            //        continue;

            //    bool startResult = false;

            //    if (m_ServiceDict.TryGetValue(serverConfig.ServiceName, out serviceType))
            //    {
            //        if (serviceType == null)
            //        {
            //            LogUtil.LogError(string.Format("The service {0} cannot be found in configuration!", serverConfig.ServiceName));
            //            LogUtil.LogError("Failed to start " + serverConfig.Name + " server!");
            //            return false;
            //        }

            //        IRunable server = Activator.CreateInstance(serviceType) as IRunable;
            //        if (server != null && server.Setup(GetServiceProvider(serverConfig.ServiceName, serverConfig.Provider), serverConfig, config.ConsoleBaseAddress))
            //        {
            //            //server.ServerCredentials = credentials;

            //            if (server.Start())
            //            {
            //                m_ServerList.Add(server);
            //                startResult = true;
            //            }
            //        }
            //    }

              
            //}


            return true;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public static void Stop()
        {
            //foreach (IRunable server in m_ServerList)
            //{
            //    server.Stop();
            //    LogUtil.LogInfo(server.Name + " has been stopped");
            //}
        }

      
       

    }
}
