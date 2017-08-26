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

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="scheduleID"></param>
        public WorkerController(int scheduleID)
        {
            //采集model
            collModel = CollectionInfo.CollctionModelList.Find(n => n.CollectionId == scheduleID);
            //采集插件
            plugin = new PluginGeneral(Type.GetType("NovelCollProject.plugin." + collModel.Plugin + ".Page"));
            plugin._CollectionModel = collModel;
        }


        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="bookId"></param>
        public void Execute(int interval)
        {
            Log.ShowLine(string.Format("<C{0}> 开始采集：{1}", collModel.CollectionId, collModel.Name));
            plugin.Execute(interval);
            Log.ShowLine(string.Format("<C{0}> -------------------------------本轮作业完成------------------------------", collModel.CollectionId), ConsoleColor.DarkYellow);
        }
    }
}
