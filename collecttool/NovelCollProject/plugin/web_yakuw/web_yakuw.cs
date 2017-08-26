using HtmlAgilityPack;
using NovelCollProject;
using NovelCollProjectutils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelCollProject.plugin.web_yakuw
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
            pageFeature.InjectUrlRule(PageTypeEnum.StartupPage, @"REG:yakuw.com/shu/\d+.html$");
           // pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:yxcoop.com/\d+.shtml\?chapter=");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:yakuw.com/shu/\d+/\d+.html");
            //pageFeature.InjectUrlRule(PageTypeEnum.DetailPage2, @"REG:bixia.org/\d+_\d+/\d+.html");
        }

        /// <summary>
        /// 解析页面内容
        /// </summary>
        /// <param name="htmlstring"></param>
        protected override void parseHtmlString(string htmlstring,int bookId=0)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlstring);
            switch (PageType)
            {
                case PageTypeEnum.StartupPage:
                    if(bookId<=0)
                        FinalData = parseStartupPage(document.DocumentNode);
                    else
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
            HtmlNode linkNodes = documentNode.SelectSingleNode("//meta[@property='og:title']");
            if (linkNodes != null)
            {
                var title = linkNodes.GetAttributeValue("content","");
                ht.Add(CollectionFieldName.Novel_Name, title);
            }
            linkNodes = documentNode.SelectSingleNode("//div[@class='pic text-center']/img");
            if (linkNodes != null)
            {
                var imgUrl = linkNodes?.GetAttributeValue("src", "");
                imgUrl= HTMLUtil.GetFullURL(InternalRealUrl, imgUrl);
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
            var url = InternalRealUrl;
            ht.Add(CollectionFieldName.Url, url);
            //linkNodes = documentNode.SelectSingleNode("//div[@class='book_intro']/p[@id='summary']");
            //if (linkNodes != null)
            //{
            //    var intr = linkNodes.InnerHtml;
            //    ht.Add(CollectionFieldName.Novel_Intr, intr);
            //}
            tempReg = new Regex(@"/(\d+).html");
            tempMatch = tempReg.Match(InternalRealUrl);
            if (tempMatch.Success)
            {
                ht.Add(CollectionFieldName.Novel_UniqueFlag, tempReg.Replace(tempMatch.Value,"$1"));
            }
            Hashtable returndata = new Hashtable();
            if (ht != null)
                returndata.Add(CollectionFieldName.BookInfo, ht);
            return returndata;
        }

        /// <summary>
        /// 解析列表页内容
        /// </summary>
        /// <param name="documentNode"></param>
        /// <returns></returns>
        private Hashtable parseListPage(HtmlNode documentNode)
        {
            List<Hashtable> links = null;
            List<string> multipage = null;
            Regex reg  ;
            Match m ;
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//dl[@class='zjlist']/dd");
            if (linkNodes != null)
            {
                links = new List<Hashtable>();
                foreach (var liNode in linkNodes)
                {
                    var title = liNode.SelectSingleNode("a")?.InnerText;
                    var url = liNode.SelectSingleNode("a")?.GetAttributeValue("href", "");
                    //if (url.Contains("zhangjie23079134.shtml"))
                    //{
                    //}
                    //else {
                    //    continue;
                    //}

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
            HtmlNode nextLink = documentNode.SelectNodes("//ul[@class='pager']/li/a")?.FirstOrDefault(x => x.InnerText == "下一页");
            if (nextLink != null&& links!=null&& links.Count>1)
            {

                int bookId = 0;
                reg = new Regex(@"book/(\d+)/");
                m = reg.Match(InternalRealUrl);
                if (m.Success)
                {
                    bookId = Convert.ToInt32(reg.Replace(m.Value, "$1"));
                }
                int p = 1;
                reg = new Regex(@"p=(\d+)");
                m = reg.Match(InternalRealUrl);
                if (m.Success)
                {
                    p = Convert.ToInt32(reg.Replace(m.Value, "$1")) + 1;
                }
                else
                    p++;
                var nexturl =string.Format("http://xsm.meixiangdao.com/book/{0}/chapters?p={1}&sort=asc",bookId,p);
                //if(PageFeature.MatchURL(nexturl) == PageTypeEnum.DetailPage1)
                if (!string.IsNullOrEmpty(nexturl) && nexturl != "#")
                {
                    multipage = new List<string>() { nexturl };
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
            string tempInnerText= null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@id='content']");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "div", "style", "script");
                tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString= tempString.Replace("\r\n", "").Replace("\t", "");
                returndata.Add(CollectionFieldName.Chap_Content, tempString);
                tempInnerText = HTMLUtil.RemoveHtmlTag(tempString).Replace("&nbsp;","");
                if (!string.IsNullOrEmpty(tempInnerText))
                {
                    returndata.Add(CollectionFieldName.Chap_ContentLen, tempInnerText.Length);
                    string into = "";
                    if (tempInnerText.Length > 40)
                    {
                        into = tempInnerText.Substring(0, 40) + "...";
                    }
                    else {
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
            HtmlNode nextLink = documentNode.SelectNodes("//*[@id='content']/div[@class='text']/a")?.FirstOrDefault(x => x.InnerText == "下一节");
            if (nextLink != null)
            {

                string url = nextLink.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(url) && url != "#")
                    returndata.Add(CollectionFieldName.NextUrl, url);
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
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "div", "style", "script");
                tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString = tempString.Replace("\r\n", "").Replace("\t", "");
                returndata.Add(CollectionFieldName.ExContent, tempString);
            }
            HtmlNode nextLink = documentNode.SelectNodes("//*[@id='content']/div[@class='text']/a")?.FirstOrDefault(x => x.InnerText == "下一节");
            if (nextLink != null)
            {

                string url = nextLink.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(url) && url != "#")
                    returndata.Add(CollectionFieldName.NextUrl, url);
            }
            return returndata;
        }
    }
}
