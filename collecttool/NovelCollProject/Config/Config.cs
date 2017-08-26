using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace NovelCollProject
{
    /// <summary>
    /// 全局配置文件
    /// </summary>
    public static class Config
    {
        private static string basePath = null;	//服务文件保存路径


        public static void SetConfig()
        {
            basePath = System.AppDomain.CurrentDomain.BaseDirectory;

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("basePath/log4net.config"));
        }


        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings["SubmitUrl"];
        }

    }
}
