using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using System.Drawing.Imaging;

namespace NovelCollProjectutils
{
    /// <summary>
    /// 图片上传操作类
    /// </summary>
    public static class ImgUtil
    {
        /// <summary>
        /// 获取图片上传host
        /// </summary>
        public static string PicUploadHost
        {
            get
            {
                // string _connectionString = ConfigurationManager.AppSettings["DBConnString"];
                //string _picUploadHost = ConfigurationManager.AppSettings["PicUploadHost"];
                string _picUploadHost = "http://pic.junqingtvtest.com";
                return _picUploadHost;
            }
        }

        //上传网络图片
        private static readonly string PicUploadUrl_Net = PicUploadHost + "/home/UploadNetPicWithSize";
        //上传本地图片
        private static readonly string PicUploadUrl_File = PicUploadHost + "/home/UploadImgWithSize";





        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public static ImgUploadRet UploadImag(string netUrl, params ImgFormat[] imgFormatList)
        {
            ImgUploadRet imgRet = new ImgUploadRet();
            //通过标准构造函数
            //string jsonText = "{'ret':1,'msg':'123'}";
            string jsonText = uploadNetPicByHttp(netUrl, imgFormatList);

            JObject jObject = JObject.Parse(jsonText);
            imgRet.Ret = jObject.Property("ret").Value.ToString().ToSimpleT<int>(1);
            imgRet.Msg = jObject.Property("msg").Value.ToString();
            if (imgRet.Ret == 0)
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(jObject.Property("urls").Value.ToString());

                Dictionary<int, string> dic = new Dictionary<int, string>();
                for (int i = 0; i < ja.Count; i++)
                {
                    var id = ja[i]["Key"].ToString().ToSimpleT<int>(0);
                    var uri = ja[i]["Value"].ToString();
                    dic.Add(id, uri);
                }
                imgRet.ImgUrls = dic;

                return imgRet;
            }
            else
            {
                return imgRet;
            }

        }



        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public static ImgUploadRet UploadImag(HttpPostedFileBase httpPostedFileBase, params ImgFormat[] imgFormatList)
        {
            ImgUploadRet imgRet = new ImgUploadRet();
            //通过标准构造函数
            //string jsonText = "{'ret':1,'msg':'123'}";
            string jsonText = uploadPicByHttp(httpPostedFileBase, imgFormatList);

            JObject jObject = JObject.Parse(jsonText);
            imgRet.Ret = jObject.Property("ret").Value.ToString().ToSimpleT<int>(1);
            imgRet.Msg = jObject.Property("msg").Value.ToString();
            if (imgRet.Ret == 0)
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(jObject.Property("urls").Value.ToString());

                Dictionary<int, string> dic = new Dictionary<int, string>();
                for (int i = 0; i < ja.Count; i++)
                {
                    var id = ja[i]["Key"].ToString().ToSimpleT<int>(0);
                    var uri = ja[i]["Value"].ToString();
                    dic.Add(id, uri);
                }
                imgRet.ImgUrls = dic;

                return imgRet;
            }
            else
            {
                return imgRet;
            }
        }

        /// <summary>
        /// 上传网络图片
        /// </summary>
        /// <param name="mPicUrl"></param>
        /// <param name="imgFormatList"></param>
        /// <returns></returns>
        private static string uploadNetPicByHttp(string mPicUrl, params ImgFormat[] imgFormatList)
        {
            string responseText = "";
            try
            {
                //组合imageSizes
                string imageSizes = "";
                string imageSize = "";
                foreach (ImgFormat img in imgFormatList)
                {
                    imageSize = string.Join(",", img.Id.ToString(), (int)img.FormatType, img.Width.ToString(), img.High.ToString());
                    if (!String.IsNullOrEmpty(imageSizes))
                    {
                        imageSizes = string.Join("|", imageSizes, imageSize);
                    }
                    else
                    {
                        imageSizes = imageSize;
                    }
                }


                CookieContainer cc = new CookieContainer();
                string Cookiesstr = string.Empty;

                string postdata = "picurl=" + HttpUtility.UrlEncode(mPicUrl) + "&imageSizes=" + HttpUtility.UrlEncode(imageSizes);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(PicUploadUrl_Net));

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] postdatabytes = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = postdatabytes.Length;

                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;
                // Write encoded data into request stream
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postdatabytes, 0, postdatabytes.Length);
                requestStream.Close();

                HttpWebResponse resultNet = null;


                resultNet = (HttpWebResponse)request.GetResponse();


                if (resultNet.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream mystream = resultNet.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(mystream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    responseText = "{'ret':1,'msg':'" + "fail_n,status:" + resultNet.StatusCode + ",des:" + resultNet.StatusDescription + "'}";
                }
            }
            catch (WebException webEx)
            {
                responseText = "{'ret':1,'msg':'" + "fail,status:" + webEx.Status + ",msg:" + webEx.Message + "'}";
            }
            catch (Exception e)
            {
                responseText = "{'ret':1,'msg':'" + e.Message + "'}";
            }

            return responseText;
        }

        /// <summary>
        /// 本地图片上传
        /// </summary>
        /// <param name="httpPostedFileBase"></param>
        /// <param name="imgFormatList"></param>
        /// <returns></returns>
        private static string uploadPicByHttp(HttpPostedFileBase httpPostedFileBase, params ImgFormat[] imgFormatList)
        {
            string responseText = "";
            try
            {
                Stream fs = httpPostedFileBase.InputStream;

                //组合imageSizes
                string imageSizes = "";
                string imageSize = "";
                foreach (ImgFormat img in imgFormatList)
                {
                    imageSize = string.Join(",", img.Id.ToString(), (int)img.FormatType, img.Width.ToString(), img.High.ToString());
                    if (!String.IsNullOrEmpty(imageSizes))
                    {
                        imageSizes = string.Join("|", imageSizes, imageSize);
                    }
                    else
                    {
                        imageSizes = imageSize;
                    }
                }


                CookieContainer cc = new CookieContainer();
                string Cookiesstr = string.Empty;


                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(PicUploadUrl_File));


                string boundary = "****************fD4fH3gL0hK7aI6";

                //request.AllowAutoRedirect = false;
                //request.CookieContainer = cc;
                request.Method = "POST";
                //request.KeepAlive = true;
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                MemoryStream stream = new MemoryStream();
                byte[] line = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");

                //提交文本字段

                string format = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}\r\n";

                string s = string.Format(format, "imageSize", imageSizes);
                byte[] data = Encoding.UTF8.GetBytes(s);
                stream.Write(data, 0, data.Length);
                stream.Write(line, 0, line.Length);

                //提交文件
                string fformat = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string file = string.Format(fformat, "file", httpPostedFileBase.FileName);
                byte[] filedata = Encoding.UTF8.GetBytes(file);
                stream.Write(filedata, 0, filedata.Length);

                byte[] buffer = new byte[fs.Length + 1];
                fs.Read(buffer, 0, buffer.Length);

                stream.Write(buffer, 0, buffer.Length);

                byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                stream.Write(foot_data, 0, foot_data.Length);

                request.ContentLength = stream.Length;


                Stream requestStream = request.GetRequestStream();

                stream.Position = 0L;
                stream.CopyTo(requestStream);
                stream.Close();

                requestStream.Close();


                HttpWebResponse result = null;


                result = (HttpWebResponse)request.GetResponse();


                if (result.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream mystream = result.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(mystream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    responseText = "{'ret':1,'msg':'" + "fail_n,status:" + result.StatusCode + ",des:" + result.StatusDescription + "'}";
                }
            }
            catch (WebException webEx)
            {
                responseText = "{'ret':1,'msg':'" + "fail,status:" + webEx.Status + ",msg:" + webEx.Message + "'}";
            }
            catch (Exception e)
            {
                responseText = "{'ret':1,'msg':'" + e.Message + "'}";
            }

            return responseText;
        }






        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public static ImgUploadRet UploadImag(byte[] data, string extension, params ImgFormat[] imgFormatList)
        {
            ImgUploadRet imgRet = new ImgUploadRet();
            //通过标准构造函数
            //string jsonText = "{'ret':1,'msg':'123'}";
            string jsonText = uploadPicByHttp(data, extension, imgFormatList);

            JObject jObject = JObject.Parse(jsonText);
            imgRet.Ret = jObject.Property("ret").Value.ToString().ToSimpleT<int>(1);
            imgRet.Msg = jObject.Property("msg").Value.ToString();
            if (imgRet.Ret == 0)
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(jObject.Property("urls").Value.ToString());

                Dictionary<int, string> dic = new Dictionary<int, string>();
                for (int i = 0; i < ja.Count; i++)
                {
                    var id = ja[i]["Key"].ToString().ToSimpleT<int>(0);
                    var uri = ja[i]["Value"].ToString();
                    dic.Add(id, uri);
                }
                imgRet.ImgUrls = dic;

                return imgRet;
            }
            else
            {
                return imgRet;
            }
        }
        /// <summary>
        /// 本地图片上传
        /// </summary>
        /// <param name="httpPostedFileBase"></param>
        /// <param name="imgFormatList"></param>
        /// <returns></returns>
        private static string uploadPicByHttp(byte[] uploadfiledata, string extension, params ImgFormat[] imgFormatList)
        {
            string responseText = "";
            try
            {

                //组合imageSizes
                string imageSizes = "";
                string imageSize = "";
                foreach (ImgFormat img in imgFormatList)
                {
                    imageSize = string.Join(",", img.Id.ToString(), (int)img.FormatType, img.Width.ToString(), img.High.ToString());
                    if (!String.IsNullOrEmpty(imageSizes))
                    {
                        imageSizes = string.Join("|", imageSizes, imageSize);
                    }
                    else
                    {
                        imageSizes = imageSize;
                    }
                }


                CookieContainer cc = new CookieContainer();
                string Cookiesstr = string.Empty;


                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(PicUploadUrl_File));


                string boundary = "****************fD4fH3gL0hK7aI6";

                //request.AllowAutoRedirect = false;
                //request.CookieContainer = cc;
                request.Method = "POST";
                //request.KeepAlive = true;
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                MemoryStream stream = new MemoryStream();
                byte[] line = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");

                //提交文本字段

                string format = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}\r\n";

                string s = string.Format(format, "imageSize", imageSizes);
                byte[] data = Encoding.UTF8.GetBytes(s);
                stream.Write(data, 0, data.Length);
                stream.Write(line, 0, line.Length);

                //提交文件
                string fformat = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string file = string.Format(fformat, "file", "local" + extension);
                byte[] filedata = Encoding.UTF8.GetBytes(file);
                stream.Write(filedata, 0, filedata.Length);



                stream.Write(uploadfiledata, 0, uploadfiledata.Length);

                byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                stream.Write(foot_data, 0, foot_data.Length);

                request.ContentLength = stream.Length;


                Stream requestStream = request.GetRequestStream();

                stream.Position = 0L;
                stream.CopyTo(requestStream);
                stream.Close();

                requestStream.Close();


                HttpWebResponse result = null;


                result = (HttpWebResponse)request.GetResponse();


                if (result.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream mystream = result.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(mystream))
                        {
                            responseText = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    responseText = "{'ret':1,'msg':'" + "fail_n,status:" + result.StatusCode + ",des:" + result.StatusDescription + "'}";
                }
            }
            catch (WebException webEx)
            {
                responseText = "{'ret':1,'msg':'" + "fail,status:" + webEx.Status + ",msg:" + webEx.Message + "'}";
            }
            catch (Exception e)
            {
                responseText = "{'ret':1,'msg':'" + e.Message + "'}";
            }

            return responseText;
        }

        /// <summary>
        /// 检查图片是不是动图或者长图
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckImageIsGif(string url,int minHeight, int minWidth)
        {
            int FramesCount = 0;
            try
            {
                //创建一个request 同时可以配置requst其余属性
                System.Net.WebRequest imgRequst = System.Net.WebRequest.Create(url);
                //在这里我是以流的方式保存图片
                System.Drawing.Image downImage = System.Drawing.Image.FromStream(imgRequst.GetResponse().GetResponseStream());
                FrameDimension fd = new FrameDimension(downImage.FrameDimensionsList[0]);
                FramesCount = downImage.GetFrameCount(fd);
                //图片大小
                if (downImage.Height <= minHeight|| downImage.Width<=minWidth)
                    return true;
                //动态图
                return  FramesCount > 1 ? true :false;
                ////以Jpeg格式保存各帧
                //for (int i = 0; i < count; i++)
                //{
                //    downImage.SelectActiveFrame(fd, i);
                //    downImage.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 检查图片格式
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileExtension CheckImgFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            string fileType = string.Empty; ;
            try
            {
                byte data = br.ReadByte();
                fileType += data.ToString();
                data = br.ReadByte();
                fileType += data.ToString();
                FileExtension extension;
                try
                {
                    extension = (FileExtension)Enum.Parse(typeof(FileExtension), fileType);
                }
                catch
                {

                    extension = FileExtension.VALIDFILE;
                }
                return extension;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    br.Close();
                }
            }
        }


    }






    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        PNG = 13780,
        SWF = 6787,
        RAR = 8297,
        ZIP = 8075,
        _7Z = 55122,
        VALIDFILE = 9999999
    }


    /// <summary>
    /// 图片格式
    /// </summary>
    public class ImgFormat
    {
        /// <summary>
        /// 全参构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imgFormatType"></param>
        /// <param name="width"></param>
        /// <param name="high"></param>
        public ImgFormat(int id, ImgFormatType imgFormatType, int width, int high)
        {
            this.Id = id;
            this.FormatType = imgFormatType;
            this.Width = width;
            this.High = high;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public ImgFormatType FormatType { get; set; }

        /// <summary>
        /// 图片宽度 0:不限制宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图片高度 0:不限制高度
        /// </summary>
        public int High { get; set; }
    }

    /// <summary>
    /// 图片格式化类型
    /// </summary>
    public enum ImgFormatType
    {
        /// <summary>
        /// 固定宽高
        /// </summary>
        Spec = 1,
        /// <summary>
        /// 指定高度,宽度同比例
        /// </summary>
        ScaleWith = 2,
        /// <summary>
        /// 指定宽度,高度同比例
        /// </summary>
        ScaleHigh = 3
    }



    /// <summary>
    /// 图片上传结果
    /// </summary>
    public class ImgUploadRet
    {
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public bool IsSuc
        {
            get
            {
                return this.Ret == 0;
            }
        }
        /// <summary>
        /// 取得图片地址
        /// </summary>
        public string GetImgUrl(int id)
        {
            string imgUrl = "";
            this.ImgUrls.TryGetValue(id, out imgUrl);
            return imgUrl;
        }
        /// <summary>
        /// 生成的原始图片url地址
        /// </summary>
        /// <returns></returns>
        public string GetOriginalImgUrl()
        {
            return GetImgUrl(0);
        }

        /// <summary>
        /// 上传结果 0:成功, 1:失败
        /// </summary>
        public int Ret { get; set; }

        /// <summary>
        /// 上传信息返回
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 图片上传结果url
        /// </summary>
        public Dictionary<int, string> ImgUrls { get; set; }
    }


}
