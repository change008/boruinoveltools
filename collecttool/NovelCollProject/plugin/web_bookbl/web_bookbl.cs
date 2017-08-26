using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelCollProjectutils;

namespace NovelCollProject.plugin.web_bookbl
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
            pageFeature.InjectUrlRule(PageTypeEnum.ListPage1, @"REG:bookbl.com/\w+/\d+.html\?chapterlist");
            pageFeature.InjectUrlRule(PageTypeEnum.DetailPage1, @"REG:bookbl.com/\w+/\d+/\d+.html");
            //pageFeature.InjectUrlRule(PageTypeEnum.DetailPage2, @"REG:hkslg520.com/\d+/\d+/\d+_\d+.html");
        }



        public override void Load(bool isUTF8, int bookId = 0)
        {
            if (PageFeature.MatchURL(Url) == PageTypeEnum.ListPage1)
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d23b26ea705975793cfcc09049a058b9a1503146168; UM_distinctid=15dfa7e604965c-0aa2b550cc2ef2-3f63440c-1fa400-15dfa7e604ac5a; Hm_lvt_8bde26ba5d77afdacc8a675a10f06979=1503146172,1503146180,1503146186; Hm_lpvt_8bde26ba5d77afdacc8a675a10f06979=1503147058; CNZZDATA1256784909=901161271-1503146169-null%7C1503147055");
                SetHost("www.bookbl.com");
            }
            else
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36");
                SetCookie("__cfduid=d23b26ea705975793cfcc09049a058b9a1503146168; UM_distinctid=15dfa7e604965c-0aa2b550cc2ef2-3f63440c-1fa400-15dfa7e604ac5a; Hm_lvt_8bde26ba5d77afdacc8a675a10f06979=1503146172,1503146180,1503146186; Hm_lpvt_8bde26ba5d77afdacc8a675a10f06979=1503147058; CNZZDATA1256784909=901161271-1503146169-null%7C1503147055");
                SetHost("www.bookbl.com");
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
            HtmlNodeCollection linkNodes = documentNode.SelectNodes("//ul[@class='list-group']/li[@class='list-group-item col-xs-6 col-sm-4 col-md-3 col-lg-3 vv-book']");
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
            tempNode = documentNode.SelectSingleNode("//div[@id='content']");
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
                    .Replace("WWw.ZiyoUgE.com", "")
                    .Replace("?	?t5矺?z?@2(\'|珛o[m?:sΞz嶞???5|??4", "")
                    .Replace("t5矺?z?@2(\'|珛o[m?:sΞz嶞???5|??4", "")
                    .Replace("?  ? t5矺 ? z ?@2(\'|珛o[m?:sΞz嶞???5|??4", "")
                    
    
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
