using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelCollProjectutils
{
    public class LocalizationImg
    {
        /// <summary>
        /// 文本中的图片本地化
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <returns>new_content</returns>
        public static string LocalizationImgInTxt(string content)
        {
            Regex reg = new Regex(@"<img[^>]*(data-)?src=['""\s]*([^\s'""]+)[^>]*>");
            List<string> imageList = new List<string>();

            MatchCollection matches = reg.Matches(content);
            foreach (Match m in matches)
            {
                string url = reg.Replace(m.Value, "$2");
                if (!string.IsNullOrEmpty(url) && (url.StartsWith("http://") || url.StartsWith("https://")))
                {
                    if (imageList.IndexOf(url) == -1)
                    {
                        imageList.Add(url);

                        ImgData imgObj = doImgUpLoad(url);
                        string newurl = imgObj.img_default;
                        content = content.Replace(m.Value, string.Format("<img src=\"{0}\" />", newurl));

                    }
                    else
                    {
                        //_logger.Error("----LocalizationImgInTxt---11111111111 url: " + url);
                    }
                }
                else
                {
                    //_logger.Error("----LocalizationImgInTxt---2222222222 url: " + url);
                }
            }

            return content;
        }



        //网络图片本地化上传
        /// <param name="imgUrl">图片网络地址</param>
        /// <param name="formatType">剪裁方式</param>
        /// <param name="width">宽</param>
        /// <param name="high">高</param>
        public static ImgData doImgUpLoad(string imgUrl, ImgFormatType formatType = ImgFormatType.ScaleWith, int width = 500, int high = 800)
        {
            ImgData data = new ImgData();
            //开始处理图片相关
            if (!string.IsNullOrEmpty(imgUrl) && (imgUrl.StartsWith("http://") || imgUrl.StartsWith("https://")))
            {
                //开始上传图片
                ImgUploadRet imgRet = ImgUtil.UploadImag(imgUrl, new ImgFormat(1, formatType, width, high));
                if (imgRet.IsSuc)
                {
                    data.img_default = imgRet.GetImgUrl(0); //本地化原尺寸地址
                    data.img_specify = imgRet.GetImgUrl(1); //本地化后生成的指定规格 300*225后的地址
                    data.img_original = imgUrl;
                    //_logger.Error("----doImgUpLoad--- img_original: " + imgUrl + " img_default:" + data.img_default + " img_specify:"+data.img_specify);
                }
                else //失败
                {
                    data.img_default = ""; //
                    data.img_specify = ""; //
                    data.img_original = imgUrl;
                   
                }
            }

            return data;
        }

        public class ImgData
        {
            public string img_specify { get; set; }
            public string img_default { get; set; }
            public string img_original { get; set; }
        }

    }
}
