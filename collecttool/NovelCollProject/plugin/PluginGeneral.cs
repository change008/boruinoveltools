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


        private CollectionModel _collectionModel;
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
                currentUrl_ChapterList = _collectionModel.Url;
                baseUrl = _collectionModel.Url;
                currenttask.collectionId = _collectionModel.CollectionId;
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
        private string currentUrl_ChapterList = "";

        //当前采集任务的章节具体地址
        private string currentUrl_ChapterDetail = "";

        /// <summary>
        /// 基础url
        /// </summary>
        private string baseUrl = "";

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

        int retryTimes = 0;

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
                Log.Show(string.Format("<C{0}> {1}  ", _CollectionModel.CollectionId, _CollectionModel.Name), ConsoleColor.DarkGreen);

                //如果存在章节列表地址并且当前章节列表数据为空的话,那么进行采集章节列表操作,说明上次采集到的数据已经被对比处理
                while (!string.IsNullOrEmpty(this.currentUrl_ChapterList))
                {
                    //采集章节列表数据--要一次性将章节列表数据采集完存放到CollectionModel中
                    currenttask.Url = currentUrl_ChapterList; //设置采集的章节列表页url--初始url                            
                                                              //开始执行采集操作
                                                              //currenttask.DoChapterList(_CollectionModel.IsUTF8);
                    currenttask.Load(_CollectionModel.IsUTF8);

                    //采集完成后的操作--数据填充到ConfigModel中去
                    buildModel();
                }

                //章节列表数据采集完成---请求我们的图书数据,查看当前我们自己线上数据中图书章节状态信息
                WxReceiveBookDto receiveResult = publishBiz.doGetBookInfo(this._CollectionModel.CollectionId);
                if (receiveResult.id == 0) //说明当前线上还没有此图书信息,我们要进行插入操作
                {
                    publishBiz.doSubmitBook(this._CollectionModel);
                }

                //再次获取线上图书状态数据--和当前采集到的最新章节列表数据进行比对
                receiveResult = publishBiz.doGetBookInfo(this._CollectionModel.CollectionId);
                if (receiveResult.id == 0)
                {
                    throw new Exception("线上图书信息不存在");
                }


                wxchapter nextChapter = CollectionModel.GetNextChapter(this._CollectionModel.chapterList, receiveResult.maxchapterid);
                while (nextChapter != null)
                {
                    try
                    {
                        int intervalX = new Random().Next(100, 500); //每次休息一定时间
                        //int intervalX = new Random().Next(20, 100); //每次休息一定时间

                        Thread.Sleep(intervalX);

                        //开始采集本章节内容数据
                        this.currentUrl_ChapterDetail = nextChapter.chapterUrl;
                        while (!string.IsNullOrEmpty(this.currentUrl_ChapterDetail))
                        {
                            currenttask.Url = this.currentUrl_ChapterDetail;
                            //currenttask.DoChapterDetail(_CollectionModel.IsUTF8);
                            currenttask.Load(_CollectionModel.IsUTF8);

                            //采集完成后的操作--数据填充到Model中去
                            buildModel(nextChapter);
                        }

                        publishBiz.doSubmitChapter(nextChapter);

                        //提交完后错误计数重置
                        retryTimes = 0;

                        Log.Show(string.Format("<C{0}> {1} 灌入章节数据: {2}  ", _CollectionModel.CollectionId, _CollectionModel.Name, nextChapter.Title), ConsoleColor.DarkGreen);

                        //再次获取线上图书状态数据
                        receiveResult = publishBiz.doGetBookInfo(this._CollectionModel.CollectionId);
                        nextChapter = CollectionModel.GetNextChapter(this._CollectionModel.chapterList, receiveResult.maxchapterid);


                    }
                    catch (Exception exChapter)
                    {
                        int intervalW = new Random().Next(2*1000, 8*1000); //每次休息一定时间
                        Thread.Sleep(intervalW);

                        Log.Show(string.Format("<C{0}> {1} error: {2}, chapter:{3}  ", _CollectionModel.CollectionId, _CollectionModel.Name, exChapter.Message, this.currentUrl_ChapterDetail), ConsoleColor.DarkRed);
                        Log.Show(string.Format("<C{0}> {1} retryTimes: {2}", _CollectionModel.CollectionId, _CollectionModel.Name, retryTimes)); 
                        retryTimes++;
                        if (retryTimes >=8)
                        {
                            throw exChapter;
                        } 
                    }
                }

                //一次完整的采集操作完成,现在要清理数据间隔一个比较长的时间之后再次
                Log.Show(string.Format("<C{0}> {1} 完成一次完整流程,暂时没有更新,休息24小时再次检查  ", _CollectionModel.CollectionId, _CollectionModel.Name), ConsoleColor.DarkGreen);
                //清理数据
                this._CollectionModel.chapterList = new List<wxchapter>();
                this.currentUrl_ChapterList = this._CollectionModel.Url;

                //再次启动检查操作
                Thread.Sleep(24 * 60 * 60 * 1000); //直接休息24个小时
                Execute(interval);

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
                    Log.ShowLine(string.Format("{1}<C{0}> 没有采到内容{2}", _CollectionModel.CollectionId, _CollectionModel.Name, currenttask.Url), ConsoleColor.DarkRed);
                    throw new Exception("没有采集到章节内容数据");
                }
                else if (currenttask.PageType == PageTypeEnum.ListPage1 || currenttask.PageType == PageTypeEnum.ListPage2 || currenttask.PageType == PageTypeEnum.ListPage3)
                {
                    throw new Exception("没有采集到章节列表数据");
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
            Hashtable bookInfo = hashtable.ContainsKey(CollectionFieldName.BookInfo) ? (Hashtable)hashtable[CollectionFieldName.BookInfo] : null;
            //detaiList = null;
            if (bookInfo != null)
            {

            }
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

            Hashtable htKeys = new Hashtable(); //判断是否重复

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

                    //防止出现重复章节一直循环
                    if (htKeys.ContainsKey(rm.Id))
                    {
                        continue;
                    }
                    htKeys.Add(rm.Id, rm.Id); 

                    //章节数据保存
                    this._CollectionModel.chapterList.Add(rm);
                }
            }

            this.currentUrl_ChapterList = null;

            if (nextPages != null)
            {
                foreach (string url in nextPages)
                {
                    this.currentUrl_ChapterList = GetFullURL(url);
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


            //在最后统一计算阅读价格,500字3分钱
            int price = (cm.ContentLen / 500) * 3;
            if (price > 15)
                price = 15;
            cm.Pirce = price;

        }


        /// <summary>
        /// 获取完整url地址
        /// </summary>
        /// <param name="relativeURL"></param>
        /// <returns></returns>
        protected string GetFullURL(string relativeURL)
        {
            return HTMLUtil.GetFullURL(baseUrl, relativeURL);
        }
    }
}
