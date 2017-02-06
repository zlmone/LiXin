using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Net;
using LiXinModels;
namespace SMTPApp
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
        public void SendAllEmail(Object emailInfo)
        {
            emailInfo = emailInfo as EmailInforMation;
            ThreadPool.QueueUserWorkItem(DoWorkEmail, emailInfo);
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


                    if (type == EmailType.Html)
                    {
                        message.IsBodyHtml = true;
                    }
                    else
                    {
                        message.IsBodyHtml = false;
                    }

                    if (infomation.listEmailAndContent.Count > 0)
                    {
                        foreach (var strEmailTo in infomation.listEmailAndContent)
                        {
                            SmtpClient smtp = new SmtpClient(infomation.Host, infomation.IPort);
                            smtp.Credentials = new NetworkCredential(infomation.UserName, infomation.PassWord);
                            smtp.EnableSsl = false; // 如果使用GMail，则需要设置为true 
                            //执行查询获得要发往的邮件地址 
                            //获取发送邮件主题
                            message.Body = strEmailTo.Value;
                            try
                            {
                                Console.WriteLine("正在发送" + infomation.From);
                                message.To.Clear();
                                message.To.Add(strEmailTo.Key);
                                smtp.Send(message);
                                Console.WriteLine("发送内容是:" + strEmailTo.Value + ",成功");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        return true;
                    }
                    else
                    {
                        message.Body = infomation.Context;
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
                                Console.WriteLine("发送内容是:" + infomation.Context + ",成功");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return false;
                            }
                        }
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
