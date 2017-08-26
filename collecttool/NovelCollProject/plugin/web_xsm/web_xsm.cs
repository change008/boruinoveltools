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

namespace NovelCollProject.plugin.web_xsm
{
    class Page : PageBase
    {
        private static string[] RY = new string[]
        {
            "//script"
        };

        public object HtmlUtil { get; private set; }

        public override void Load(bool isUTF8, int bookId = 0)
        {
            if (PageFeature.MatchURL(Url) == PageTypeEnum.DetailPage2)
            {
                SetUserAgent("Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
                SetCookie("BookShelf=%5B14438%5D; Hm_lvt_3e218c50fbbc6f691dedf0e179c40693=1488791695; Hm_lvt_6dbf080831d4941496a1a4a144cd658e=1488435997,1489116418; _auth_=%7B%22uid%22%3A%22661245%22%2C%22nick%22%3A%22%5Cu6e29%5Cu67d4%5Cu54e5%22%2C%22mobile%22%3A%2218310471284%22%7D; uc=94NXVE7HLN1XjFKmbuP484q%2ByuWOCk7GTJM5Nz5t1NdG%2FUr6%2BIs30Mm4YCFmXoVJjXzMkqy%2F450wDGCopXk7lO944VNho4WHSfuuNuuphiu6Y4seGNbJTD%2FsKDXeO0OIjxqMObrTHIy3fx8Z7QOWkzRyacB0kYlOKJUApDSSFdf4LszNAY%2FhWtT7lSCFYqryWZPW4zU6G4CbUnRwA9qbv%2BB5hHLqCN3LFxUGjUuVHut4%2FHB0t6IZCBj9MXWmik815BH9eiPAPyHRZECRumgbcXxYOrmVrLTauC8piVay5nc%3D; ReadConfig=%7B%22theme%22%3A3%2C%22fontsize%22%3A26%2C%22offline%22%3A1%7D; Hm_lvt_e4959332a15f318672a50251a3d3949c=1488860410,1489116409,1489482328,1489563488; Hm_lpvt_e4959332a15f318672a50251a3d3949c=1489641589; RecentRead=%5B%5B14298%2C1016231%2C38%5D%2C%5B14065%2C845358%2C41%5D%2C%5B14438%2C1042173%2C28%5D%2C%5B14077%2C854107%2C21%5D%2C%5B14096%2C866915%2C1%5D%5D; LastRead=%7B%22book_id%22%3A14298%2C%22book_name%22%3A%22%E8%B0%83%E6%95%99%E5%A8%87%E5%A6%BB%EF%BC%9A%E6%88%91%E7%9A%84%E8%85%B9%E9%BB%91%E6%80%BB%E8%A3%81%22%2C%22chapter_id%22%3A1016231%2C%22chapter_code%22%3A38%2C%22chapter_title%22%3A%22%E7%AC%AC38%E7%AB%A0%E3%80%80%E6%88%91%E7%AB%9F%E6%97%A0%E8%A8%80%E4%BB%A5%E5%AF%B9%22%7D");
                SetHost("user.xsm.meixiangdao.com");
            }
            base.Load(isUTF8, bookId);
        }

        /// <summary>
        ///添加页面特征规则 
        /// </summary>
        /// <param name="pageFeature"></param>
        protected override void buildPageFeatureRules(PageFeature pageFeature)
        {
            pageFeature.InjectUrlRule(PageTypeEnum.StartupPage, @"REG:/book/\d+.html/");
            pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:book/\d+/chapters");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:book/\d+/\d+.html");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage2, @"REG:/vip/\d+/\d+.html");
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
            HtmlNode linkNodes = documentNode.SelectSingleNode("//div[@class='book_title wrap']");
            if (linkNodes != null)
            {
                var title = linkNodes.InnerText;
                ht.Add(CollectionFieldName.Novel_Name, title);
            }
            linkNodes = documentNode.SelectSingleNode("//div[@class='panel']");
            if (linkNodes != null)
            {
                var imgUrl = linkNodes.SelectSingleNode("div[@class='fn-clear']/img")?.GetAttributeValue("src", "");
                ht.Add(CollectionFieldName.Novel_CoverImgs, imgUrl);
                var tag = linkNodes.SelectSingleNode("div[@class='fn-clear']/ul/li[2]/a")?.InnerText;
                ht.Add(CollectionFieldName.Novel_Tag, tag);
                var statusName = linkNodes.SelectSingleNode("div[@class='fn-clear']/ul/li[3]")?.InnerText;
                if (!string.IsNullOrEmpty(statusName) && statusName.Contains("完结"))
                    ht.Add(CollectionFieldName.Novel_Status, BookStatus.BookStatus_Finish);
                else
                    ht.Add(CollectionFieldName.Novel_Status, BookStatus.BookStatus_Update);
                var ContentLenStr = linkNodes.SelectSingleNode("div[@class='fn-clear']/ul/li[4]")?.InnerText;
                int ContentLen = 0;
                if (!string.IsNullOrEmpty(ContentLenStr))
                {
                    tempReg = new Regex(@"(\d+.\d+)|(\d+)");
                    tempMatch = tempReg.Match(ContentLenStr);
                    if (tempMatch.Success)
                    {
                        if (ContentLenStr.Contains("万"))
                            ContentLen = (int)(Convert.ToDouble(tempMatch.Value) * 10000);
                        if (ContentLenStr.Contains("千"))
                            ContentLen = (int)(Convert.ToDouble(tempMatch.Value) * 1000);
                    }
                    ht.Add(CollectionFieldName.Novel_ContentLen, ContentLen);
                }

            }
            linkNodes = documentNode.SelectSingleNode("//div[@class='panel']/a[@class='btn block white']");
            if (linkNodes != null)
            {
                var url = linkNodes.GetAttributeValue("href", "");
                ht.Add(CollectionFieldName.Url, url);
            }
            linkNodes = documentNode.SelectSingleNode("//div[@class='book_intro']/p[@id='summary']");
            if (linkNodes != null)
            {
                var intr = linkNodes.InnerHtml;
                ht.Add(CollectionFieldName.Novel_Intr, intr);
            }
            tempReg = new Regex(@"/book/(\d+).html");
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
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//ul[@class='menu']/li");
            if (linkNodes != null)
            {
                links = new List<Hashtable>();
                foreach (var liNode in linkNodes)
                {
                    var title = liNode.SelectSingleNode("a").InnerText;
                    var url = liNode.SelectSingleNode("a").GetAttributeValue("href", "");
                     reg = new Regex(@"(\d+).html");
                     m = reg.Match(url);
                    string flag = reg.Replace(m.Value, "$1");
                    Hashtable ht = new Hashtable();
                    ht.Add(CollectionFieldName.Chap_Title, title);
                    ht.Add(CollectionFieldName.Url, url);
                    ht.Add(CollectionFieldName.Chap_UniqueFlag, flag);
                    reg = new Regex(@"第(\d+)章");
                    m = reg.Match(title);
                    if (m.Success)
                    {
                        int order = Convert.ToInt32(reg.Replace(m.Value, "$1"));
                        ht.Add(CollectionFieldName.Chap_SortOrder, order);
                    }

                    links.Add(ht);
                }
            }
            HtmlNode nextLink = documentNode.SelectNodes("//ul[@class='pager']/li/a").FirstOrDefault(x => x.InnerText == "下一页");
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
            Hashtable returndata = new Hashtable();
            HtmlNode tempNode = null;
            string tempString = null;
            string tempInnerText= null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@class='content']");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "style", "script");
                returndata.Add(CollectionFieldName.Chap_Content, tempString);
                tempInnerText = tempNode.InnerText;
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
                }
                returndata.Add(CollectionFieldName.Chap_Status, ChapterStatus.ChapterStatus_OnLine);
                returndata.Add(CollectionFieldName.Chap_ChapterType, ChapterType.ChapterType_Free);
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
            Hashtable returndata = new Hashtable();
            HtmlNode tempNode = null;
            string tempString = null;
            string tempInnerText = null;
            Regex tempReg = null;
            Match tempMatch = null;
            tempNode = documentNode.SelectSingleNode("//div[@class='content']");
            if (tempNode != null)
            {
                tempString = tempNode.InnerHtml;
                tempString = HTMLUtil.RemoveHtmlTag(tempString, "p", "img", "br");
                tempString = HTMLUtil.RemoveHtmlContent(tempString, "style", "script");
                returndata.Add(CollectionFieldName.Chap_Content, tempString);
                tempInnerText = tempNode.InnerText;
                if (!string.IsNullOrEmpty(tempInnerText))
                {
                    int price = (tempString.Length / 1000) * 5;
                    if (price == 0)
                        price = 5;
                    if (price > 15)
                        price = 15;
                    returndata.Add(CollectionFieldName.Chap_ContentLen, tempString.Length);
                    returndata.Add(CollectionFieldName.Chap_Pirce, price);
                }
                returndata.Add(CollectionFieldName.Chap_Status, ChapterStatus.ChapterStatus_OnLine);
                returndata.Add(CollectionFieldName.Chap_ChapterType, ChapterType.ChapterType_Pay);
            }
            return returndata;
        }
    }
}
