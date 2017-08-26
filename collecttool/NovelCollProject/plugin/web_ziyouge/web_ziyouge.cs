using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelCollProjectutils;

namespace NovelCollProject.plugin.web_ziyouge
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
            pageFeature.InjectUrlRule(PageTypeEnum.StartupPage, @"REG:ziyouge.com/nonono$");
            pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:ziyouge.com/\w+/\d+/\d+/index.html\?chapterlist");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:ziyouge.com/\w+/\d+/\d+/\d+.html");
            //pageFeature.InjectUrlRule(PageTypeEnum.DetailPage2, @"REG:hkslg520.com/\d+/\d+/\d+_\d+.html");
        }


        public override void Load(bool isUTF8, int bookId = 0)
        {
            if (PageFeature.MatchURL(Url) == PageTypeEnum.ListPage1)
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d4eea6ba24aed93761f72640003caa8741499590833; cf_clearance=8517be9645768b7e96357adceac88c30ed77c82c-1503065784-28800; yunsuo_session_verify=2b42ea40965fa1009ccdfc189a9b02c6; Hm_lvt_a77691cb730c8dcad98ba43d333f5063=1503064397,1503064412,1503064659,1503064680; Hm_lpvt_a77691cb730c8dcad98ba43d333f5063=1503065794");
                SetHost("www.ziyouge.com");
            }
            else
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d4eea6ba24aed93761f72640003caa8741499590833; cf_clearance=8517be9645768b7e96357adceac88c30ed77c82c-1503065784-28800; yunsuo_session_verify=a87746c42f9b04cc7c6cdb88e64b40c0; Hm_lvt_a77691cb730c8dcad98ba43d333f5063=1503064397,1503064412,1503064659,1503064680; Hm_lpvt_a77691cb730c8dcad98ba43d333f5063=1503065851");
                SetHost("www.ziyouge.com");
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
            HtmlNode linkNodes = documentNode.SelectSingleNode("//uv[@class='chapter-list']/li[@class='chapter']");
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
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//ul[@class='chapter-list']/li[@class='chapter']");
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
                    //if (!string.IsNullOrEmpty(title))
                    //{
                    //    reg = new Regex(@"第(\d+)章");
                    //    m = reg.Match(title);
                    //    if (m.Success)
                    //    {
                    //        int order = Convert.ToInt32(reg.Replace(m.Value, "$1"));
                    //        ht.Add(CollectionFieldName.Chap_SortOrder, order);
                    //    }
                    //}
                    //else
                    //{
                    //}


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
            List<string> multipage = null;
            Hashtable returndata = new Hashtable();
            HtmlNode tempNode = null;
            string tempString = null;
            string tempInnerText = null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@id=\"htmlContent\"]");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "div", "style", "script");
                //tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString = tempString.Replace("\r\n", "").Replace("\t", "")
                    .Replace("本站访问地址http://www.ziyouge.com 任意搜索引擎内输入:紫幽阁 即可访问!", "")
                    .Replace("http://www.ziyouge.com", "")
                    .Replace("紫幽阁", "")
                    .Replace("www.ziyouge.com", "")
                    .Replace("ziyouge.com", "")
                    .Replace("ziyouge", "")
                    .Replace("http://", "")
                    .Replace("http", "")
                    .Replace("紫Ｙou阁 ＷwＷ.ZiyouＧＥ.com", "")
                    .Replace("WWw.ZiyoUgE.com", "")
                   
                    ;

                //正则替换域名
                string pattern = @"(?=.{3,255}$)[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+";
                tempString = Regex.Replace(tempString, pattern, "");


                returndata.Add(CollectionFieldName.Chap_Content, tempString);

                //移除无效字符,用来计算长度
                tempInnerText = HTMLUtil.RemoveHtmlTag(tempString).Replace("&nbsp;", "").Replace("feisuz", "")
                    .Replace("作者的话:", "").Replace("新书，求收藏求推荐", "").Replace("本书红薯网首发,请勿转载!", "");
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
