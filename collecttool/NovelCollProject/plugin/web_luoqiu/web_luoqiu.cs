using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelCollProjectutils;

namespace NovelCollProject.plugin.web_luoqiu
{
    class Page : PageBase
    {
        private static string[] RY = new string[]
        {
            "//script"
        };

        public object HtmlUtil { get; private set; }



        /// <summary>
        ///添加页面特征规则 
        /// </summary>
        /// <param name="pageFeature"></param>
        protected override void buildPageFeatureRules(PageFeature pageFeature)
        {
            pageFeature.InjectUrlRule(PageTypeEnum.StartupPage, @"REG:lwtxt.net/nonono$");
            pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:luoqiu.com/\w+/\d+/\?chapterlist");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:luoqiu.com/\w+/\d+/\d+.html");
        }

        public override void Load(bool isUTF8, int bookId = 0)
        {
            if (PageFeature.MatchURL(Url) == PageTypeEnum.ListPage1)
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d12efff8d5351e89b3c31e3b4b99bcf621503147450; UM_distinctid=15dfa91f9fa4ad-0f1be94c203b7a-3f63440c-1fa400-15dfa91f9fbb67; scrollspeed=5; CNZZDATA1257366169=1877948417-1503147026-null%7C1503206426");
                SetHost("www.luoqiu.com");
            }
            else
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d12efff8d5351e89b3c31e3b4b99bcf621503147450; UM_distinctid=15dfa91f9fa4ad-0f1be94c203b7a-3f63440c-1fa400-15dfa91f9fbb67; scrollspeed=5; CNZZDATA1257366169=1877948417-1503147026-null%7C1503206426");
                SetHost("www.luoqiu.com");
            }
            base.Load(isUTF8, bookId);
        }


        /// <summary>
        /// 解析页面内容
        /// </summary>
        /// <param name="htmlstring"></param>
        protected override void parseHtmlString(string htmlstring, int bookId = 0)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlstring);
            switch (PageType)
            {
                case PageTypeEnum.StartupPage:
                    FinalData = parseStartupPage(document.DocumentNode);
                    break;
                case PageTypeEnum.ListPage1:
                    FinalData = parseListPage(document.DocumentNode);
                    break;
                case PageTypeEnum.DetailPage1:
                    FinalData = parseDetailPage1(document.DocumentNode);
                    break;
                case PageTypeEnum.DetailPage2:
                    FinalData = parseDetailPage2(document.DocumentNode);
                    break;
            }
        }

        /// <summary>
        /// 解析开始页
        /// </summary>
        /// <param name="documentNode"></param>
        /// <returns></returns>
        private Hashtable parseStartupPage(HtmlNode documentNode)
        {
            Hashtable ht = new Hashtable();
            Regex tempReg = null;
            Match tempMatch = null;
            HtmlNode linkNodes = documentNode.SelectSingleNode("//div[@id='info']/h1");
            if (linkNodes != null)
            {
                var title = linkNodes.InnerText;
                ht.Add(CollectionFieldName.Novel_Name, title);
            }
            linkNodes = documentNode.SelectSingleNode("//div[@id='fmimg']/img");
            if (linkNodes != null)
            {
                var imgUrl = linkNodes?.GetAttributeValue("src", "");
                ht.Add(CollectionFieldName.Novel_CoverImgs, imgUrl);
            }
            linkNodes = documentNode.SelectSingleNode("//meta[@property='og:novel:category']");
            if (linkNodes != null)
            {
                var tag = linkNodes?.GetAttributeValue("content", "");
                ht.Add(CollectionFieldName.Novel_Tag, tag);
            }    
            var url = InternalRealUrl + "?chapter=";
            ht.Add(CollectionFieldName.Url, url);

            tempReg = new Regex(@"/(\w+)/$");
            tempMatch = tempReg.Match(InternalRealUrl);
            if (tempMatch.Success)
            {
                ht.Add(CollectionFieldName.Novel_UniqueFlag, tempReg.Replace(tempMatch.Value, "$1"));
            }
            Hashtable returndata = new Hashtable();
            if (ht != null)
                returndata.Add(CollectionFieldName.BookInfo, ht);
            return returndata;
        }

        /// <summary>
        /// 解析列表页内容--解析章节列表数据
        /// </summary>
        /// <param name="documentNode"></param>
        /// <returns></returns>
        private Hashtable parseListPage(HtmlNode documentNode)
        {
            List<Hashtable> links = null;
            List<string> multipage = null;
            Regex reg;
            Match m;
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//div[@class='dccss']");
            if (linkNodes != null)
            {
                links = new List<Hashtable>();
                foreach (var liNode in linkNodes)
                {
                    var title = liNode.SelectSingleNode("a")?.InnerText;
                    var url = liNode.SelectSingleNode("a")?.GetAttributeValue("href", "");
                    if (string.IsNullOrEmpty(url))
                        continue;
                    reg = new Regex(@"(\d+).html");
                    m = reg.Match(url);
                    string flag = reg.Replace(m.Value, "$1");
                    Hashtable ht = new Hashtable();
                    ht.Add(CollectionFieldName.Chap_Title, title);
                    ht.Add(CollectionFieldName.Url, url);
                    ht.Add(CollectionFieldName.Chap_UniqueFlag, flag);
                    if (!string.IsNullOrEmpty(title))
                    {
                        reg = new Regex(@"第(\d+)章");
                        m = reg.Match(title);
                        if (m.Success)
                        {
                            int order = Convert.ToInt32(reg.Replace(m.Value, "$1"));
                            ht.Add(CollectionFieldName.Chap_SortOrder, order);
                        }
                    }
                    else
                    {
                    }


                    links.Add(ht);
                }
            }
            Hashtable returndata = new Hashtable();
            if (links != null)
                returndata.Add(CollectionFieldName.Items, links);
            if (multipage != null)
                returndata.Add(CollectionFieldName.Pages, multipage);
            return returndata;
        }
        /// <summary>
        /// 解析明细页内容
        /// </summary>
        /// <param name="documentNode"></param>
        /// <returns></returns>
        private Hashtable parseDetailPage1(HtmlNode documentNode)
        {

            System.Threading.Thread.Sleep(new Random((int)DateTime.Now.Ticks).Next(8000, 30000));

            List<string> multipage = null;
            Hashtable returndata = new Hashtable();
            HtmlNode tempNode = null;
            string tempString = null;
            string tempInnerText = null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@id='content']");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "div", "style", "script","center","span");

                tempString = tempString.Replace("\r\n", "").Replace("\t", "");
                returndata.Add(CollectionFieldName.Chap_Content, tempString);

                //移除无效字符,用来计算长度
                tempInnerText = HTMLUtil.RemoveHtmlTag(tempString).Replace("&nbsp;", "");
                if (!string.IsNullOrEmpty(tempInnerText))
                {
                    returndata.Add(CollectionFieldName.Chap_ContentLen, tempInnerText.Length);
                    string into = "";
                    if (tempInnerText.Length > 40)
                    {
                        into = tempInnerText.Substring(0, 40) + "...";
                    }
                    else
                    {
                        into = tempInnerText;
                    }
                    returndata.Add(CollectionFieldName.Chap_Intro, into);
                    int price = (tempString.Length / 1000) * 5;
                    if (price == 0)
                        price = 5;
                    if (price > 15)
                        price = 15;
                    returndata.Add(CollectionFieldName.Chap_Pirce, price);
                }
                returndata.Add(CollectionFieldName.Chap_Status, ChapterStatus.ChapterStatus_OnLine);
                returndata.Add(CollectionFieldName.Chap_ChapterType, ChapterType.ChapterType_Free);
                tempInnerText = tempNode.InnerText;

            }
            else
            {
                int i = 1;
                int j = 1 + 1;
           
            }
            return returndata;
        }

        /// <summary>
        /// 解析明细页内容
        /// </summary>
        /// <param name="documentNode"></param>
        /// <returns></returns>
        private Hashtable parseDetailPage2(HtmlNode documentNode)
        {
            return null;
        }
    }
}
