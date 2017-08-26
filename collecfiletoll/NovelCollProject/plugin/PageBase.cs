using System;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using NovelCollProject.net;
using NovelCollProject.utils;
using System.Text.RegularExpressions;
using log4net;
using NovelCollProjectutils;

namespace NovelCollProject.plugin
{
    class PageBase
    {
        public PageTypeEnum PageType;
        public Hashtable FinalData;

        protected string InternalUrl;
        protected string InternalRealUrl;
        protected URLLoader UrlLoader;
        protected PageFeature PageFeature;
        protected string PostValue;
        private static ILog _logger = LogManager.GetLogger(typeof(PageBase));
        public string Url
        {
            set
            {
                InternalUrl = value;

                if (PageFeature == null)
                {
                    PageFeature = new PageFeature();
                    buildPageFeatureRules(PageFeature);
                }

                PageType = PageFeature.MatchURL(InternalUrl);
            }
            get { return InternalUrl; }
        }
        /// <summary>
        /// 必须要重写
        /// </summary>
        /// <param name="pageFeature"></param>
        protected virtual void buildPageFeatureRules(PageFeature pageFeature)
        {
        }

        /************************************/
        public void SetUserAgent(string userAgent)
        {
            if (UrlLoader == null)
                UrlLoader = new URLLoader();

            UrlLoader.SetUserAgent(userAgent);
        }

        public void SetCookie(string cookie)
        {
            if (UrlLoader == null)
                UrlLoader = new URLLoader();

            UrlLoader.SetHttpHeader("Cookie", cookie);
        }
        public void SetReferer(string value)
        {
            UrlLoader.SetHttpHeader("Referer", value);
        }

        public void SetHost(string value)
        {
            PostValue = value;
            UrlLoader.SetHttpHeader("Host", value);
        }
        public void SetAccept_Encoding(string value)
        {
            if (UrlLoader == null)
                UrlLoader = new URLLoader();

            UrlLoader.SetHttpHeader("Accept-Encoding", value);
        }

        public void SetHttpHeader(string key, string value)
        {
            if (UrlLoader == null)
                UrlLoader = new URLLoader();
            UrlLoader.SetHttpHeader(key, value);
        }

        public virtual void SetPostBody(string post)
        {
            if (UrlLoader == null)
                UrlLoader = new URLLoader();

            UrlLoader.SetPostBody(post);
        }

        /************************************/

        public virtual void Load(bool isUTF8, int bookId =0)
        {
            Regex reg = new Regex(@"POST:\[(.+?)\]");
            Match m = reg.Match(InternalUrl);
            if (m.Success) SetPostBody(reg.Replace(m.Value, "$1"));

            InternalRealUrl = getRealUrl(InternalUrl);

            if (string.IsNullOrEmpty(InternalRealUrl))
                throw new Exception("没有初始化Url设置");

            if (UrlLoader == null)
                UrlLoader = new URLLoader();

            string result = string.Empty;

            Log.ShowLine("请求：" + InternalRealUrl, ConsoleColor.DarkGray);

            if (isUTF8) result = UrlLoader.RequestByUTF8(InternalRealUrl);
            else result = UrlLoader.RequestByGBK(InternalRealUrl);

            if (!string.IsNullOrEmpty(result))
            {
                if (PageType == PageTypeEnum.NONE)
                    PageType = PageFeature.MatchHtml(result);

                if (PluginGeneral.DEBUG_MODE)
                {
                    parseHtmlString(result, bookId);
                }
                else
                {
                    try
                    {
                        parseHtmlString(result, bookId);
                    }
                    catch (Exception exp)
                    {
                        FinalData = null;
                        Log.ShowLine(exp, ConsoleColor.Red);
                        _logger.FatalFormat("访问网页：{0}。出错{1}。返回内容为空", InternalRealUrl, exp.Message);
                    }
                }
            }
            else
            {
                FinalData = null;
                Log.ShowLine(InternalRealUrl + " 错误~!", ConsoleColor.Red);
                _logger.FatalFormat("访问网页：{0}。出错。返回内容为空", InternalRealUrl);
            }
        }

        protected virtual void parseHtmlString(string htmlstring, int bookId = 0) { }

        protected virtual string getRealUrl(string interUrl)
        {
            Regex reg = new Regex(@"POST:\[(.+?)\]");
            Match m = reg.Match(InternalUrl);
            if (m.Success) return reg.Replace(interUrl, "");

            return interUrl;
        }
        /************************************/
        protected static void clearNoice(HtmlNode source, string[] noices)
        {
            foreach (string s in noices)
            {
                HtmlNodeCollection ryn = source.SelectNodes(s);
                if (ryn != null) foreach (HtmlNode nnn in ryn) if (nnn.ParentNode != null) nnn.ParentNode.RemoveChild(nnn);
            }
        }

        protected static string getValueFromJS(string htmlstring, string key)
        {
            List<Regex> regList = new List<Regex>()
            {
                new Regex("\\s" + key + @"\s*=\s*""([^""]+)"""),
                new Regex("\\s" + key + @"\s*=\s*'([^']+)'"),
                new Regex("\\s" + key + @"\s*=\s*(\d+)"),
                new Regex("\"" + key + "\"\\s*:\\s*\"([^\"]+)\"")
            };

            foreach (Regex reg in regList)
            {
                Match match = reg.Match(htmlstring);
                if (match.Success)
                {
                    string value = reg.Replace(match.Value, "$1");
                    return value.Trim();
                }
            }

            return null;

        }

        protected string getBaseUrl(HtmlNode documentNode = null)
        {
            string baseUrl = null;
            if (documentNode != null)
            {
                HtmlNode baseNode = documentNode.SelectSingleNode("//base[@href]");
                if (baseNode != null)
                {
                    baseUrl = baseNode.GetAttributeValue("href", "");
                }
            }

            if (string.IsNullOrEmpty(baseUrl))
            {
                Uri uri = new Uri(InternalRealUrl);
                baseUrl = uri.Scheme + "://" + uri.Host;
            }

            return baseUrl;
        }

        /// <summary>
        /// 图片相对路径改为全路径
        /// </summary>
        /// <param name="documentNode"></param>
        /// <param name="InternalRealUrl"></param>
        protected static void ReplaceIncorrectImageSrc(HtmlNode documentNode, string InternalRealUrl)
        {
            HtmlNodeCollection imgNode = documentNode.SelectNodes("//img");
            if (imgNode == null) return;
            foreach (var item in imgNode)
            {
                var url = item.GetAttributeValue("src", "");
                var realUrl = HTMLUtil.GetFullURL(InternalRealUrl, url);
                item.SetAttributeValue("src", realUrl);
                //documentNode.InnerHtml=documentNode.InnerHtml.Replace(url, realUrl);
            }
        }

        /// <summary>
        /// 修改占位图片路径
        /// </summary>
        /// <param name="documentNode"></param>
        /// <param name="InternalRealUrl"></param>
        protected static void ReplacePlaceholderImageSrc(HtmlNode documentNode, string InternalRealUrl, string PlaceholderAttribute)
        {
            HtmlNodeCollection imgNode = documentNode.SelectNodes("//img");
            if (imgNode == null) return;
            foreach (var item in imgNode)
            {
                var url = item.GetAttributeValue(PlaceholderAttribute, "");
                if (!string.IsNullOrEmpty(url))
                {
                    var realUrl = HTMLUtil.GetFullURL(InternalRealUrl, url);
                    item.SetAttributeValue("src", realUrl);
                    item.Attributes.Remove(PlaceholderAttribute);
                }
            }
        }

        private void dealHtmlstring(ref string Htmlstring)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(Htmlstring);
            HtmlNode htmlNode = document.DocumentNode;
            //清洗多余标签
            htmlNode.InnerHtml = HTMLUtil.RemoveHtmlTag(htmlNode.InnerHtml, "p", "vedio", "img", "br");
        }


        /// <summary>
        /// 检测文章是否是num天以内的
        /// </summary>
        /// <param name="postTime"></param>
        /// <returns></returns>
        protected static bool checkPostTime(string postTime, int num)
        {
            bool result = true;
            DateTime time = DateTime.Now;
            if (!DateTime.TryParse(postTime, out time))
            {
                return result;
            }
            if (DateTime.Compare(time.AddDays(num), DateTime.Now) < 0)
                return false;
            return result;
        }

        /// <summary>
        /// 检查内容中的图片是否包含gif图或者不符合格式的图片(高度小于60像素或者宽度小于 120像素)
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        protected bool FilterPicture(string url, int minHeight = 120, int minWidth = 120)
        {
            bool result = false;
            if (url.ToLower().Contains("gif") || ImgUtil.CheckImageIsGif(url, minHeight, minWidth))
                return true;
            return result;
        }

    }
}
