using log4net;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace NovelCollProject.net
{
    public class URLLoader : WebClient
    {
        public const string USERAGENT_PC_CHROME = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.111 Safari/537.36";
        public const string USERAGENT_IOS = "Mozilla/5.0 (iPhone; CPU iPhone OS 8_0 like Mac OS X) AppleWebKit/600.1.3 (KHTML, like Gecko) Version/8.0 Mobile/12A4345d Safari/600.1.4";
        public const string USERAGENT_ANDROID = "Mozilla/5.0 (Linux; Android 4.2.2; GT-I9505 Build/JDQ39) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.59 Mobile Safari/537.36";
        public const string USERAGENT_SPIDER = "Mozilla/5.0 (compatible; iaskspider/1.0; MSIE 6.0)";
        private static ILog _logger = LogManager.GetLogger(typeof(URLLoader));
        /// <summary>
        /// 单位毫秒
        /// </summary>
        public int TimeOutValue = 7 * 60 * 1000;

        public bool AllowAutoRedirect = true;

        private string postBody;

        public void SetUserAgent(string agent)
        {
            SetHttpHeader("User-Agent",agent);
        }

        public void SetPostBody(string value)
        {
            postBody = value;
        }

        public void SetHttpHeader(string key, string value)
        {
            if (Headers[key] != null)
            {
                Headers[key] = value;
            }
            else
            {
                Headers.Add(key,value);
            }
        }

        public string RequestByGBK(string url)
        {
            return RequestByEncoding(url, Encoding.GetEncoding("gb2312"));
        }

        public string RequestByUTF8(string url)
        {
            return RequestByEncoding(url, Encoding.UTF8);
        }

        public string RequestByEncoding(string url, Encoding encoder)
        {
            byte[] postBytes = null;
            if (!String.IsNullOrEmpty(postBody))
            {
                //SetHttpHeader("Content-Type", "application/x-www-form-urlencoded");
                SetHttpHeader("Content-Type", "application/json;charset=UTF-8");
                postBytes = encoder.GetBytes(postBody);
            }

            byte[] bResult = null;

            int timecounter = 0;
            while (bResult == null && timecounter++ < 100)
            {
                try
                {
                    if (postBytes == null)
                    {
                        bResult = DownloadData(url);
                    }
                    else
                    {
                        bResult = UploadData(url, "POST", postBytes);
                    }
                }
                catch (WebException exp)
                {
                    _logger.FatalFormat("网站访问出错。状态{0},错误消息：{1}", (int)exp.Status, exp.Message);


                    if (exp.Status == WebExceptionStatus.ConnectFailure ||
                        exp.Status == WebExceptionStatus.Timeout)
                    {
                        //Log.ShowLine(url+" 异常，重新尝试！", ConsoleColor.Red);
                    }
                    else
                    {
                        //Log.ShowLine(url+" 异常，不再尝试！", ConsoleColor.Red);
                        break;
                    }
                }
                catch (NotSupportedException exp)
                {
                    break;
                }
            }

            string result = null;
            if (bResult != null)
            {
                if (ResponseHeaders["Content-Encoding"] != null && ResponseHeaders["Content-Encoding"].ToLower() == "gzip")
                {
                    MemoryStream ms = new MemoryStream(bResult);
                    MemoryStream msTemp = new MemoryStream();
                    GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);
                    StreamReader sr = new StreamReader(gzip, encoder);
                    result = sr.ReadToEnd();
                }
                else
                {
                    result = encoder.GetString(bResult);
                }
                
            }
            return result;
        }
        
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = TimeOutValue;
            request.AllowAutoRedirect = AllowAutoRedirect;
            //request.ReadWriteTimeout = TimeOutValue;
            //request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            return request;
        }
    }
}
