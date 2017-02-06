using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Mail;
using LiXinModels;
using System.Net;
using LiXinInterface;


namespace LiXinBLL
{
    public class Email 
    {
        private void DoWorkEmail(object EmailClass)
        {
            EmailInforMation emailInfo = (EmailInforMation)EmailClass;
            SendEmail(emailInfo, EmailType.Html);
        }
        /// <summary>
        /// 群发邮件的方法
        /// </summary>
        /// <param name="emailInfo">邮件的基础信息</param>
        public void SendAllEmail(EmailInforMation emailInfo)
        {
            Thread t = new Thread(new ParameterizedThreadStart(DoWorkEmail));
            t.Start(emailInfo);
        }

        public bool SendEmail(EmailInforMation infomation, EmailType type)
        {
            try
            {
                try
                {
                    MailMessage message = new MailMessage();
                    //获取发送者地址
                    message.From = new MailAddress(infomation.UserName);
                    //获取主题
                    message.Subject = infomation.Subject;
                    //获取发送邮件主题
                    message.Body = infomation.Context;





                    if (type == EmailType.Html)
                    {
                        message.IsBodyHtml = true;
                    }
                    else
                    {
                        message.IsBodyHtml = false;
                    }

                    foreach (string strEmailTo in infomation.ToList)
                    {
                        SmtpClient smtp = new SmtpClient(infomation.Host, infomation.IPort);
                        smtp.Credentials = new NetworkCredential(infomation.UserName, infomation.PassWord);
                        smtp.EnableSsl = false; // 如果使用GMail，则需要设置为true 
                        //执行查询获得要发往的邮件地址  
                        try
                        {
                            message.To.Clear();
                            message.To.Add(strEmailTo);
                            smtp.Send(message);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        
    }
}
