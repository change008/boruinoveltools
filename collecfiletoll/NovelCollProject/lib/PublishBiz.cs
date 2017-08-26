using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovelCollProject.model;
using NovelCollProject.net;

namespace NovelCollProject
{
    /// <summary>
    /// 小说发布操作--包括发布和请求
    /// </summary>
    public class PublishBiz
    {
        private List<string> specialStr = new List<string>();
        public PublishBiz()
        {
            //添加需要替换的特殊字符
            specialStr.Add("&nbsp");
            specialStr.Add(" ");
            specialStr.Add("\r\n");
            specialStr.Add("\r");
            specialStr.Add("\n");
            specialStr.Add("&quot");
            specialStr.Add(",");
            specialStr.Add(".");
            specialStr.Add("?");
            specialStr.Add(";");
            specialStr.Add("，");
            specialStr.Add("。");
            specialStr.Add("？");
            specialStr.Add("；");
        }


        /// <summary>
        /// 请求图书信息数据,根据采集id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public WxReceiveBookDto doGetBookInfo(int collectionId)
        {
            string url = Constants.Get_Url_GetBookInfo_Full();
            string result = doRequest(url + "?collectionid=" + collectionId);
            if (result != null)
            {
                var dto = JsonConvert.DeserializeObject<WxReceiveBookDto>(result);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// 提交书籍信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WxReceiveBookDto doSubmitBook(CollectionModel model)
        {
            WxCollectBookDto dto = WxCollectBookDto.fromCollectionModel(model);
            return doSubmitBook(dto);
        }

        
        public WxReceiveBookDto doSubmitBook(WxCollectBookDto model)
        {
            string url = Constants.Get_Url_SubmitBook_Full();
            string result = doPost(url, model);
            if (result != null)
            {
                var dto = JsonConvert.DeserializeObject<WxReceiveBookDto>(result);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// 提交章节内容信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WxReceiveBookDto doSubmitChapter(wxchapter model)
        {
            WxCollectChapterDto dto = WxCollectChapterDto.fromChapter(model);
            return doSubmitChapter(dto);
        }

        public WxReceiveBookDto doSubmitChapter(WxCollectChapterDto model)
        {
            string url = Constants.Get_Url_SubmitChapter_Full();
            string result = doPost(url, model);
            if (result != null)
            {
                var dto = JsonConvert.DeserializeObject<WxReceiveBookDto>(result);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// post方式提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="smodel"></param>
        /// <returns></returns>
        private string doPost(string url, object smodel)
        {
            string jsonstr = JsonConvert.SerializeObject(smodel);
            jsonstr = HttpUtility.UrlEncode(jsonstr, System.Text.Encoding.UTF8);
            
            URLLoader loader = new URLLoader();
         
            loader.SetPostBody(jsonstr);

            string response = loader.RequestByUTF8(url);

            return response;
        }

        /// <summary>
        /// get方式请求网址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string doRequest(string url)
        {
            URLLoader loader = new URLLoader();
            string response = loader.RequestByUTF8(url);
            return response;
        }



        private string ReplaceSpecialStr(string value, List<string> specialStr)
        {
            string resultStr = value;
            foreach (var item in specialStr)
            {
                resultStr = resultStr.Replace(item, "");
            }
            return resultStr;
        }
    }

    class ResponseStatus
    {
        public bool ok { get; set; }
        public ResponseStatusEnum status { get; set; }
        public string msg { get; set; }
    }
}
