using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelCollProjectutils;

namespace NovelCollProject.plugin.web_hkslg520
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
            pageFeature.InjectUrlRule(PageTypeEnum.StartupPage, @"REG:hkslg520.com/nonono$");
            pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:hkslg520.com/\d+/\d+/\?chapterlist");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:hkslg520.com/\d+/\d+/\d+.html");
            //pageFeature.InjectUrlRule(PageTypeEnum.DetailPage2, @"REG:hkslg520.com/\d+/\d+/\d+_\d+.html");
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
            //linkNodes = documentNode.SelectSingleNode("//meta[@property='og:novel:category']");
            //if (linkNodes != null)
            //{
            //    var statusName = linkNodes?.GetAttributeValue("content","");
            //    if (!string.IsNullOrEmpty(statusName) && statusName.Contains("完结"))
            //        ht.Add(CollectionFieldName.Novel_Status, BookStatus.BookStatus_Finish);
            //    else
            //        ht.Add(CollectionFieldName.Novel_Status, BookStatus.BookStatus_Update);
            //}
            //linkNodes = documentNode.SelectSingleNode("//meta[@property='og:novel:category']");
            //if (linkNodes != null)
            //{ 
            //    var ContentLenStr = linkNodes.SelectSingleNode("div[@class='fn-clear']/ul/li[4]")?.InnerText;
            //    int ContentLen = 0;
            //    if (!string.IsNullOrEmpty(ContentLenStr))
            //    {
            //        tempReg = new Regex(@"(\d+.\d+)|(\d+)");
            //        tempMatch = tempReg.Match(ContentLenStr);
            //        if (tempMatch.Success)
            //        {
            //            if (ContentLenStr.Contains("万"))
            //                ContentLen = (int)(Convert.ToDouble(tempMatch.Value) * 10000);
            //            if (ContentLenStr.Contains("千"))
            //                ContentLen = (int)(Convert.ToDouble(tempMatch.Value) * 1000);
            //        }
            //        ht.Add(CollectionFieldName.Novel_ContentLen, ContentLen);
            //    }

            //}
            //linkNodes = documentNode.SelectSingleNode("//div[@class='panel']/a[@class='btn block white']");
            //if (linkNodes != null)
            //{
            //    var url = linkNodes.GetAttributeValue("href", "");
            //    ht.Add(CollectionFieldName.Url, url);
            //}
            var url = InternalRealUrl + "?chapter=";
            ht.Add(CollectionFieldName.Url, url);
            //linkNodes = documentNode.SelectSingleNode("//div[@class='book_intro']/p[@id='summary']");
            //if (linkNodes != null)
            //{
            //    var intr = linkNodes.InnerHtml;
            //    ht.Add(CollectionFieldName.Novel_Intr, intr);
            //}
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
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//div[@class=\"dccss\"]");
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
            List<string> multipage = null;
            Hashtable returndata = new Hashtable();
            HtmlNode tempNode = null;
            string tempString = null;
            string tempInnerText = null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@id='content']/p");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "div", "style", "script");
                //tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString = tempString.Replace("\r\n", "").Replace("\t", "").Replace("手机请访问：:feisuz", "")
                    .Replace("feisuz", "").Replace("作者的话:", "").Replace("新书，求收藏求推荐", "").Replace("本书红薯网首发,请勿转载!", "");
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
