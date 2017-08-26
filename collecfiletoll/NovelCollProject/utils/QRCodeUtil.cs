using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ZXing;

namespace NovelCollProjectutils
{
    public static class QRCodeUtil
    {


        /// <summary>
        /// 检测图片中是否包含二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsQRCodePic(string url)
        {
            bool result = false;
            try
            {
                System.Net.WebRequest imgRequest = System.Net.WebRequest.Create(url);
                System.Drawing.Image  downImg = System.Drawing.Image.FromStream(imgRequest.GetResponse().GetResponseStream());
                //使用ZXing识别二维码
                BarcodeReader reader = new BarcodeReader();
                reader.Options.CharacterSet = "UTF-8";
                var map = new Bitmap(downImg);
                Result resultStr = reader.Decode(map);
                var decodedString = resultStr == null ? "" : resultStr.Text;

                if (!string.IsNullOrEmpty(decodedString))
                    return true;
                else
                {
                    //使用QRCode.Codec识别二维码
                    QRCodeDecoder decoder = new QRCodeDecoder();
                    decodedString = decoder.decode(new QRCodeBitmapImage(map));
                        if (!string.IsNullOrEmpty(decodedString))
                        return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return result;
        }
    }
}
