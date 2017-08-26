using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Timers;
using System.Xml;
using System.Net;

namespace NovelCollProjectutils
{
    

    public static class MyEmail
    {

        private static bool IsSend = false;

        /// <summary>
        /// 邮件提醒
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendEmail(string subject = "绵羊采集器灌数据错误", string body= "同志们：绵羊采集器灌数据时出错。错误信息为：")
        {

            bool flag = true;
            try
            {
                if (!IsSend)
                {
                    SmtpClient _smtpClient = new SmtpClient();
                    //指定电子邮件发送方式
                    _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //指定SMTP服务器
                    _smtpClient.Host = "smtp.163.com";
                    //用户名和客户端授权码（邮箱密码为tx123456） 
                    _smtpClient.Credentials = new System.Net.NetworkCredential("tiexuecaijiqi@163.com", "tx1234567");                                                                                       //MailMessage _mailMessage = new MailMessage(strfrom, strto);
                    MailAddress _from = new MailAddress("tiexuecaijiqi@163.com", "采集器");//发件人
                    MailAddress _to = new MailAddress("zhangxiaowei@tiexue.com");//收件人
                    MailMessage _mailMessage = new MailMessage(_from, _to);
                    _mailMessage.CC.Add(new MailAddress("zhangqiang@tiexue.com"));//抄送
                    _mailMessage.CC.Add(new MailAddress("tianjun@tiexue.com"));//抄送
                    _mailMessage.CC.Add(new MailAddress("qiaofangjie@tiexue.com"));//抄送
                    _mailMessage.Subject = subject;//主题
                    _mailMessage.Body = body;//内容
                    _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码
                                                                             //_mailMessage.IsBodyHtml = true;//设置为HTML格式
                    _mailMessage.Priority = MailPriority.Normal;//优先级
                    _smtpClient.Send(_mailMessage);

                    //最多发送一次
                    IsSend = true;
                }

            }
            catch (Exception exception)
            {
                flag = false;
            }
            
            return flag;
        }
    }
}
