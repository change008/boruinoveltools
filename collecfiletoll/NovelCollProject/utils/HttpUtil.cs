using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovelCollProject.net;

namespace NovelCollProject.utils
{
    public class HttpUtil
    {
        public Hashtable FinalData;

        public string InternalUrl;
        public string InternalRealUrl;
        protected URLLoader UrlLoader;
       
        protected string PostValue;
      

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

        public virtual void Load(bool isUTF8)
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

            if (isUTF8) result = UrlLoader.RequestByUTF8(InternalRealUrl);
            else result = UrlLoader.RequestByGBK(InternalRealUrl);


            if (!string.IsNullOrEmpty(result))
            {
                  parseHtmlString(result);
            }     
        }

        protected virtual void parseHtmlString(string htmlstring) { }

        protected virtual string getRealUrl(string interUrl)
        {
            Regex reg = new Regex(@"POST:\[(.+?)\]");
            Match m = reg.Match(InternalUrl);
            if (m.Success) return reg.Replace(interUrl, "");

            return interUrl;
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
    }
}
