using NovelCollProject.control;
using NovelCollProject.net;
using NovelCollProjectutils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovelCollProject
{
    class Program
    {
        private static int interval;
        private static int time;
        private const string HELP = "启动参数说明：\n\n" +
                                    "-c 数字1,数字2...    —— 执行采集任务。数字1，数字2分别代表采集任务ID（ScheduleID）\n\n" +
                                    "-i 数字  —— 每次采集之间的时间间隔，单位是秒\n\n" +
                                    "-o 字符串    —— 日志文件输出目录" +
                                    "-t     —— 采集一次的时间间隔\n\n" +
                                    "-h 显示帮助文字";

        static void Main(string[] args)
        {
            //日志
            Config.SetConfig();
            time = 0;
            Console.WindowWidth = 130;
            Console.BufferHeight = 1500;
            Console.Title = "绵羊采集发布器";

            List<string> argslist = args.ToList();

            //显示帮助文字
            int index = argslist.IndexOf("-h");
            if (index > -1)
            {
                return;
            }


            //是否自动运行
            bool isAutoRun = argslist.IndexOf("-a") > -1;

            //时间间隔
            index = argslist.IndexOf("-i");
            if (index > -1)
            {
                index += 1;
                if (index < argslist.Count && argslist[index].IndexOf("-") == -1)
                {
                    interval = Convert.ToInt32(argslist[index]) * 1000;
                }
            }
            //日志文件输出目录
            index = argslist.IndexOf("-o");
            if (index > -1)
            {
                index += 1;
                if (index < argslist.Count && argslist[index].IndexOf("-") == -1)
                {
                    Log.OutputFilePath = argslist[index];
                }
            }
            //日志显示时间
            Log.ShowTime = true; //argslist.IndexOf("-st") > -1;



            //采集时间间隔（分钟）
            index = argslist.IndexOf("-t");
            if (index > -1)
            {
                index += 1;
                if (index < argslist.Count && argslist[index].IndexOf("-") == -1)
                {
                    int res;
                    if (int.TryParse(argslist[index], out res))
                        time = res;
                }
            }

            Console.Title = string.Empty;

            //采集任务
            index = argslist.IndexOf("-c");
            if (index > -1)
            {
                index += 1;
                if (index < argslist.Count && argslist[index].IndexOf("-") == -1)
                {
                    string[] ids = argslist[index].Split(',');
                    Console.Title += "采集任务 ID = " + string.Join(",", ids) + "  ";
                    doCollection(ids);
                }
            }


            //暂停主线程
            if (string.IsNullOrEmpty(Console.Title))
            {
                Console.WriteLine("没事可做~~~~任意键关闭");
                Console.ReadKey();
            }
            else
            {
                while (true)
                {
                    Thread.Sleep(60 * 60 * 1000);
                }
            }
        }

        /******************************************/

        /// <summary>
        /// 执行采集操作
        /// </summary>
        /// <param name="ids"></param>
        private static void doCollection(string[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                int id = Convert.ToInt32(ids[i]);

                ParameterizedThreadStart parStart = new ParameterizedThreadStart(_startCollectionTask);
                Thread worker1 = new Thread(parStart);
                worker1.Name = "采集器（ID=" + id + "）";
                worker1.IsBackground = true;
                worker1.Start(id);

                Thread.Sleep(interval);
            }
        }

        private static void _startCollectionTask(object scheduleID)
        {
            WorkerController controller = new WorkerController(Math.Abs((int)scheduleID));
            controller.Execute(interval);
            //while (true)
            //{
            //    WorkerController controller = new WorkerController(Math.Abs((int)scheduleID));
            //    controller.SetUpdateOnly(updateOnly);
            //    controller.Execute(interval,bookId);
            //    if (interval > 0)
            //    {
            //        if (time > 0)
            //        {
            //            int sleep = 1000 * 60 * time;
            //            Console.WriteLine("{2}<C{0}> {1}分钟后再次执行作业", scheduleID, time, updateOnly ? "*" : "");
            //            Thread.Sleep(sleep);
            //        }
            //        else
            //        {
            //            int sleep = updateOnly ? interval * 30 : interval * 100;
            //            Console.WriteLine("{2}<C{0}> {1}秒后再次执行作业", scheduleID, sleep / 1000, updateOnly ? "*" : "");
            //            Thread.Sleep(sleep);
            //        }
            //    }
            //    else
            //        break;
            //}
        }
    }
}
