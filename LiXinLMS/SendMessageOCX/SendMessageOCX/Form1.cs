using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMPPLib;
using RabbitMQ.Common;
using LiXinModels;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace SendMessageOCX
{
    public partial class Form1 : Form
    {
        Thread drawThread = null;
        public int status = 0;
        public string accountID = AccountBase.AccountId;
        public string pwd = AccountBase.Password;
        StringBuilder sb = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }



        public void AddText()
        {
            Task t = new Task(() =>
            {
                while (true)
                {
                    this.textBox1.Text = sb.ToString();
                }
            });
            t.Start();
        }

        //加载的时候进行首次连接
        private void Form1_Load(object sender, EventArgs e)
        {
            status = this.axEmpp1.ConnectServer(accountID, pwd);
           // AddText();
            //为了保险再次进行连接
            while (!(status == 0 || status == 20000))
            {
                status = this.axEmpp1.ConnectServer(accountID, pwd);
            }
           // sb.Append("已连接到短信发送服务器");
            this.textBox1.Text = "已连接到短信发送服务器";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.button1.Text = "监听中";
            Thread t = new Thread(() => Run());
            t.Start();
        }



        //开始连接消息队列
        public void Run()
        {
            RabbitMQHelper.Instance.SendMessage(null);
            RabbitMQHelper.Instance.ReceivingMessagesPassive(send);
        }


        //开始发送短信
        public void send(object mess)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                if (mess != null)
                {
                    SmsMessage model = (SmsMessage)mess;
                    var config = model.config;
                    reConnection();
                    //开始发送
                    if (status == 0 || status == 20000)
                    {
                        foreach (var item in model.list)
                        {
                            var result = 1;
                            if (item.Value.Length > 52)
                            {
                                var newstr = item.Value.Trim().Replace("\r\n", "").Replace("\n", "");
                                var sum = Math.Ceiling(newstr.Length / 52.0);
                                for (int i = 0; i < sum; i++)
                                {
                                    var content = string.Join("", newstr.Skip(i * 52).Take(52).ToList());
                                    reConnection();

                                    Task t = new Task(() =>
                                    {
                                        this.axEmpp1.Refresh();
                                        result = this.axEmpp1.SendSms(content + "(" + (i + 1) + "/" + sum + ")", item.Key);
                                    });
                                    t.Start();
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                Task t = new Task(() =>
                                {
                                    this.axEmpp1.Refresh();
                                    result = this.axEmpp1.SendSms(item.Value, item.Key);
                                });
                                t.Start();
                            }

                            var message = "";
                            if (result == 0)
                            {
                                message = "\r\n发送给" + item.Key + " 内容为:" + item.Value + "，发送成功";
                                //sb.Append(message);
                                this.textBox1.AppendText(message);
                                LogHelper.WriteLog(message);
                               
                            }
                            else
                            {
                                message = "\r\n发送给" + item.Key + " 内容为:" + item.Value + "，发送失败,错误代码：" + result + ",当前连接状态为：" + status;
                               // sb.Append(message);
                                this.textBox1.AppendText(message);
                                LogHelper.WriteLog(message);
                            }
                        }
                    }
                    else
                    {
                        this.textBox1.AppendText("连接失败");
                        LogHelper.ErrorLog("连接失败");
                    }
                }
            }
            catch (Exception ex)
            {
                this.textBox1.AppendText("出现异常，请咨询管理员");
                LogHelper.ErrorLog("出现异常，请咨询管理员");
            }
        }

        #region 公共方法

        //设置texbox的值
        // public delegate void SetTextHandler(string text);
        private void SetText(string text)
        {
            this.textBox1.AppendText(text);
            //if (this.textBox1.InvokeRequired == true)
            //{
            //    SetTextHandler set = new SetTextHandler(SetText);//委托的方法参数应和SetText一致
            //    this.textBox1.BeginInvoke(set, new object[] { text }); //此方法第二参数用于传入方法,代替形参text
            //}
            //else
            //{
            //    this.textBox1.AppendText(text);

            //}
        }


        //结束子线程  
        private void closeThread()
        {
            if (drawThread != null)
            {
                if (drawThread.IsAlive)
                {
                    drawThread.Abort();
                }
            }
        }

        private void reConnection()
        {
            //为了保险再次进行连接
            while (!(status == 0 || status == 20000))
            {
                status = this.axEmpp1.ConnectServer(accountID, pwd);
            }
        }
        #endregion


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // t.Dispose();
            Application.Exit();
            Process.GetCurrentProcess().Kill();//杀死原有进程
        }
    }
}
