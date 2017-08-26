using System;
using System.Collections;
using System.Collections.Generic;
using NovelCollProject.model;
using NovelCollProject.net;
using System.Threading;
using NovelCollProjectutils;

namespace NovelCollProject.plugin
{
    class PluginGeneral
    {
        public const bool DEBUG_MODE = false;
        public const bool SAVEABLE = true;

        public const string END_FLAG = "single_complete";
        /***********************/


        private CollectionModel _collectionModel = new CollectionModel();
        /// <summary>
        /// 相当于是书籍信息--可存放章节列表数据
        /// </summary>
        public CollectionModel _CollectionModel
        {
            get
            {
                return _collectionModel;
            }
            set
            {
                _collectionModel = value;
                CurrentUrl_ChapterList = _collectionModel.Url;
            }
        }

        /// <summary>
        /// 发布需求
        /// </summary>
        private PublishBiz publishBiz = new PublishBiz();

        //解析数据
        protected PageBase currenttask;


        //存放小说章节列表临时数据,分页的话可以多次组装
        private Dictionary<string, wxchapter> tempData;


        //采集网站
        private Type _pageType;


        //当前采集任务的章节列表地址
        public string CurrentUrl_ChapterList;

        //当前采集任务的章节具体地址
        private string currentUrl_ChapterDetail = "";

        /// <summary>
        /// 基础url
        /// </summary>
        public string BaseUrl;

        /// <summary>
        /// 类别名称
        /// </summary>
        public string sortName;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="pageType"></param>
        public PluginGeneral(Type pageType)
        {
            _pageType = pageType;
            currenttask = _pageType.Assembly.CreateInstance(_pageType.FullName) as PageBase;
            currenttask.SetUserAgent(URLLoader.USERAGENT_PC_CHROME);
        }

        /// <summary>
        /// 实际执行操作
        /// </summary>
        /// <param name="interval"></param>
        public void Execute(int interval)
        {
            try
            {
                //休息片刻
                if (interval > 0)
                {
                    int step = (int)Math.Round(interval * 0.3);
                    int interval2 = new Random().Next(interval - step, interval + step);
                    Thread.Sleep(interval2);
                }

                Thread.Sleep(30 * 1000);


                //采集图书基本信息数据/ id,标题,简介
                currenttask.Url = BaseUrl;
                currenttask.PageType = PageTypeEnum.StartupPage;


                currenttask.Load(_CollectionModel.IsUTF8);
                //采集完成后的操作--数据填充到ConfigModel中去
                buildModel();


                //写文件name和intro
                LocalFileIO.writeName(this._CollectionModel.Name, this.sortName);
                LocalFileIO.writeIntro(this._CollectionModel.Name, this._CollectionModel.Intr, this.sortName);



                //如果存在章节列表地址并且当前章节列表数据为空的话,那么进行采集章节列表操作,说明上次采集到的数据已经被对比处理
                while (!string.IsNullOrEmpty(this.CurrentUrl_ChapterList))
                {
                    //采集章节列表数据--要一次性将章节列表数据采集完存放到CollectionModel中
                    currenttask.Url = CurrentUrl_ChapterList; //设置采集的章节列表页url--初始url                            
                                                              //开始执行采集操作
                                                              //currenttask.DoChapterList(_CollectionModel.IsUTF8);
                    currenttask.PageType = PageTypeEnum.ListPage1;
                    currenttask.Load(_CollectionModel.IsUTF8);

                    //采集完成后的操作--数据填充到ConfigModel中去
                    buildModel();
                }

                //章节列表数据采集完成---请求我们的图书数据,查看当前我们自己线上数据中图书章节状态信息
                foreach (var item in this._CollectionModel.chapterList)
                {
                    int intervalX = new Random().Next(10 * 1000, 60 * 1000); //每次休息一定时间
                    Thread.Sleep(intervalX);

                    //开始采集本章节内容数据
                    this.currentUrl_ChapterDetail = item.chapterUrl;
                    while (!string.IsNullOrEmpty(this.currentUrl_ChapterDetail))
                    {
                        currenttask.Url = this.currentUrl_ChapterDetail;
                        //currenttask.DoChapterDetail(_CollectionModel.IsUTF8);
                        currenttask.Load(_CollectionModel.IsUTF8);

                        //采集完成后的操作--数据填充到Model中去
                        buildModel(item);
                    }

                    //publishBiz.doSubmitChapter(nextChapter);
                    //保存本章数据
                    string realcontent = item.Content.Replace("<br>", System.Environment.NewLine)
                        .Replace("<p>", System.Environment.NewLine)
                        .Replace("</p>", System.Environment.NewLine)
                        .Replace("&nbsp;", " ")
                        ;

                    LocalFileIO.writeChpater(this._CollectionModel.Name, item.Title, realcontent, this.sortName);

                    Log.Show(string.Format("<C> {0} 写入章节数据: {1}  ",  _CollectionModel.Name, item.Title), ConsoleColor.DarkGreen);

                }

                //一次完整的采集操作完成,现在要清理数据间隔一个比较长的时间之后再次
                Log.Show(string.Format("<C{0}> {1} 完成一次完整流程,暂时没有更新,休息24小时再次检查  ", _CollectionModel.CollectionId, _CollectionModel.Name), ConsoleColor.DarkGreen);
                //清理数据
                this._CollectionModel.chapterList = new List<wxchapter>();
                this.CurrentUrl_ChapterList = this._CollectionModel.Url;
            }
            catch (Exception ex)
            {
                Log.Show(string.Format("<C{0}> {1} error: {2}  ", _CollectionModel.CollectionId, _CollectionModel.Name, ex.Message), ConsoleColor.DarkRed);
            }
        }



        /// <summary>
        /// 提取结果数据到数据对象中
        /// </summary>
        private void buildModel(wxchapter model = null)
        {
            if (currenttask.FinalData == null || currenttask.FinalData.Count == 0)
            {
                if (currenttask.PageType == PageTypeEnum.DetailPage1 || PageTypeEnum.DetailPage2 == currenttask.PageType || PageTypeEnum.DetailPage3 == currenttask.PageType)
                {
                    if (tempData.ContainsKey(currenttask.Url))
                    {
                        tempData.Remove(currenttask.Url);
                    }
                    Log.ShowLine(string.Format("{1}<C{0}> 没有采到内容{2}", _CollectionModel.CollectionId, _CollectionModel.Name, currenttask.Url), ConsoleColor.DarkRed);
                }
                else if (currenttask.PageType == PageTypeEnum.ListPage1 || currenttask.PageType == PageTypeEnum.ListPage2 || currenttask.PageType == PageTypeEnum.ListPage3)
                {
                    throw new Exception("没有采集到章节数据");
                }
                return;
            }

            switch (currenttask.PageType)
            {
                case PageTypeEnum.StartupPage:
                    parseStartupPage(currenttask.FinalData);
                    //parseListPage(currenttask.FinalData);
                    break;
                case PageTypeEnum.ListPage1:
                case PageTypeEnum.ListPage2:
                case PageTypeEnum.ListPage3:
                    parseListPage(currenttask.FinalData);
                    break;
                case PageTypeEnum.DetailPage1:
                case PageTypeEnum.DetailPage2:
                case PageTypeEnum.DetailPage3:
                    parseDetailPage(currenttask.FinalData, model);
                    break;
            }
        }

        /// <summary>
        /// 获取小说页面内容
        /// </summary>
        /// <param name="hashtable"></param>
        private void parseStartupPage(Hashtable hashtable)
        {
            string bookName = hashtable.ContainsKey(CollectionFieldName.Novel_Name) ? (string)hashtable[CollectionFieldName.Novel_Name] : "";
            string intro = hashtable.ContainsKey(CollectionFieldName.Novel_Intr) ? (string)hashtable[CollectionFieldName.Novel_Intr] : "";

            this._CollectionModel.Name = bookName;
            this._CollectionModel.Intr = intro;
        }


        /// <summary>
        /// 章节目录列表数据
        /// </summary>
        /// <param name="hashtable"></param>
        private void parseListPage(Hashtable hashtable)
        {
            List<Hashtable> detaiList = hashtable.ContainsKey(CollectionFieldName.Items) ? (List<Hashtable>)hashtable[CollectionFieldName.Items] : null;
            List<string> nextPages = hashtable.ContainsKey(CollectionFieldName.Pages) ? (List<string>)hashtable[CollectionFieldName.Pages] : null;
            List<string> multiPages = hashtable.ContainsKey(CollectionFieldName.MultiPages) ? (List<string>)hashtable[CollectionFieldName.MultiPages] : null;

            //detaiList = null;
            if (detaiList != null)
            {
                foreach (Hashtable cm in detaiList)
                {
                    //continue;
                    string url = (string)cm[CollectionFieldName.Url];
                    if (!HTMLUtil.IsCorrect(url)) continue;
                    url = GetFullURL(url);
                    cm[CollectionFieldName.Url] = url;
                    wxchapter rm = new wxchapter();
                    fillWxChapterModel(rm, cm, _CollectionModel.CollectionId);

                    //章节数据保存
                    this._CollectionModel.chapterList.Add(rm);
                }
            }

            this.CurrentUrl_ChapterList = null;

            if (nextPages != null)
            {
                foreach (string url in nextPages)
                {
                    this.CurrentUrl_ChapterList = GetFullURL(url);
                }
            }
        }

        /// <summary>
        /// 章节详情数据提取,这里只是将章节内容存放到对应章节model中去
        /// </summary>
        /// <param name="cm"></param>
        private void parseDetailPage(Hashtable cm, wxchapter model)
        {
            //model.Content += ((string)cm[CollectionFieldName.ExContent]).Trim();
            fillWxChapterModel(model, cm, _collectionModel.CollectionId);

            if (cm.ContainsKey(CollectionFieldName.ExContent))
            {
                model.Content += ((string)cm[CollectionFieldName.ExContent]).Trim();
            }

            this.currentUrl_ChapterDetail = null;

            if (cm.ContainsKey(CollectionFieldName.NextUrl))
            {
                string pageurl = (string)cm[CollectionFieldName.NextUrl];
                pageurl = GetFullURL(pageurl);
                this.currentUrl_ChapterDetail = pageurl;
            }
        }


        /// <summary>
        /// 组装章节内容
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="cm"></param>
        /// <param name="source"></param>
        /// <param name="configID"></param>
        private void fillWxChapterModel(wxchapter cm, Hashtable source, int collectionId)
        {
            if (source.ContainsKey(CollectionFieldName.Chap_UniqueFlag)) cm.Id = (string)source[CollectionFieldName.Chap_UniqueFlag];
            if (source.ContainsKey(CollectionFieldName.Chap_Intro)) cm.Intro = (string)source[CollectionFieldName.Chap_Intro];
            if (source.ContainsKey(CollectionFieldName.Chap_SortOrder)) cm.SortOrder = (int)source[CollectionFieldName.Chap_SortOrder];
            if (source.ContainsKey(CollectionFieldName.Chap_Title)) cm.Title = (string)source[CollectionFieldName.Chap_Title];
            if (source.ContainsKey(CollectionFieldName.Chap_ChapterType)) cm.ChapterType = (int)source[CollectionFieldName.Chap_ChapterType];
            if (source.ContainsKey(CollectionFieldName.Chap_Pirce)) cm.Pirce = (int)source[CollectionFieldName.Chap_Pirce];
            //if (source.ContainsKey(CollectionFieldName.Chap_Remark)) cm.Remark = (string)source[CollectionFieldName.Chap_Remark];
            if (source.ContainsKey(CollectionFieldName.Chap_Content)) cm.Content = (string)source[CollectionFieldName.Chap_Content];
            //if (source.ContainsKey(CollectionFieldName.Chap_Status)) cm.Status = (int)source[CollectionFieldName.Chap_Status];
            if (source.ContainsKey(CollectionFieldName.Chap_ContentLen)) cm.ContentLen = (int)source[CollectionFieldName.Chap_ContentLen];

            //章节对应的章节内容地址
            if (source.ContainsKey(CollectionFieldName.Url)) cm.chapterUrl = (string)source[CollectionFieldName.Url];
            cm.CollectionId = collectionId;
        }


        /// <summary>
        /// 获取完整url地址
        /// </summary>
        /// <param name="relativeURL"></param>
        /// <returns></returns>
        protected string GetFullURL(string relativeURL)
        {
            return HTMLUtil.GetFullURL(BaseUrl, relativeURL);
        }
    }
}
