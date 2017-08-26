using System;
using NovelCollProject.model;
using NovelCollProject.plugin;
using NovelCollProject.utils;
using System.Collections.Generic;
using NovelCollProject.net;
using Newtonsoft.Json;
using log4net;
using NovelCollProjectutils;


/// <summary>
/// 采集 Worker
/// </summary>
namespace NovelCollProject.control
{
    public class WorkerController
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static ILog _logger = LogManager.GetLogger(typeof(WorkerController));

        //插件
        private PluginGeneral plugin;

        //采集model配置
        private CollectionModel collModel;

        string thisurl = "";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="scheduleID"></param>
        public WorkerController(int scheduleID)
        {
            //采集model
            //collModel = CollectionInfo.CollctionModelList.Find(n => n.CollectionId == scheduleID);
            //采集插件
            plugin = new PluginGeneral(Type.GetType("NovelCollProject.plugin." + collModel.Plugin + ".Page"));
            plugin._CollectionModel = collModel;
        }

        public WorkerController(string url)
        {
            //采集插件
            plugin = new PluginGeneral(Type.GetType("NovelCollProject.plugin.web_ziyouge.Page"));
            plugin.BaseUrl = url;
            plugin.CurrentUrl_ChapterList = url;
        }

        public WorkerController(string sortName, string url)
        {
            //采集插件
            plugin = new PluginGeneral(Type.GetType("NovelCollProject.plugin.web_ziyouge.Page"));
            plugin.BaseUrl = url;
            plugin.sortName = sortName;
            plugin.CurrentUrl_ChapterList = url;
        }





        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="bookId"></param>
        public void Execute(int interval)
        {
            Log.ShowLine(string.Format("开始采集：{0}", plugin.BaseUrl));
            plugin.Execute(interval);
            Log.ShowLine(string.Format("<C> -------------------------------本轮作业完成------------------------------"), ConsoleColor.DarkYellow);
        }
    }
}
