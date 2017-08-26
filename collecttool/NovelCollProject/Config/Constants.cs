using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelCollProjectutils;

namespace NovelCollProject
{
    /// <summary>
    /// 常量配置文件
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// 获取图书信息
        /// </summary>
        /// <returns></returns>
        public static string Get_Url_GetBookInfo_Full()
        {
            return Config.GetConfig("SubmitUrl") + Url_GetBookInfo;
        }

        /// <summary>
        /// 提交图书信息
        /// </summary>
        /// <returns></returns>
        public static string Get_Url_SubmitBook_Full()
        {
            return Config.GetConfig("SubmitUrl") + Url_SubmitBook;
        }

        /// <summary>
        /// 提交章节信息
        /// </summary>
        /// <returns></returns>
        public static string Get_Url_SubmitChapter_Full()
        {
            return Config.GetConfig("SubmitUrl") + Url_SubmitChapter;
        }


        /// <summary>
        /// 根据图书采集id查询当前图书状态url,包括最大章节等返回数据为json对象--WxReceiveBookDto
        /// </summary>
        private const string Url_GetBookInfo = "wxreceive/getbookinfo";

        /// <summary>
        /// 提交json对象WxCollectBookDto将数据灌入远程--返回对象为json对象--WxReceiveBookDto
        /// </summary>
        private const string Url_SubmitBook = "wxreceive/receivebook";

        /// <summary>
        /// 提交json对象WxCollectChapterDto将数据灌入远程--返回对象为json对象--WxReceiveBookDto
        /// </summary>
        private const string Url_SubmitChapter = "wxreceive/receivechapter";

    }
}
