using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EMPPLib;
using LiXinBLL.SystemManage;
using LiXinDataAccess.SystemManage;
using LiXinModels;
using LiXinModels.SystemManage;

namespace LiXinBLL
{
    public class SendMessage
    {
        private static SMSConfig config = new SMSConfig();
        private static EMPPLib.emptcl empp = new EMPPLib.emptclClass();
        private bool _isConnecting;

        #region 属性
        public string message
        {
            get;
            set;
        }
        public List<string> telephone
        {
            get;
            set;
        }
        #endregion


        public SendMessage(SmsMessage smess)
        {
            message = smess.message;
            telephone = smess.telePhoneList;
           // GetConfig(smess.config);
        }

        public void SendAllMessage()
        {
            Thread t = new Thread(() => SendMessages());
            t.Start();
        }


        /// <summary>
        /// 获取配置
        /// </summary>
        public void GetConfig(Sys_ParamConfig _config = null)
        {
            config = (new Sys_ParamConfigBL()).GetSmsConfing(_config);
        }



        //检查连接状态 && (tryTimes++ < 10)
        public void CheckConnection()
        {
            if (_isConnecting)
            {
                return;//避免循环递归
            }
            _isConnecting = true;
            if (!empp.connected)
            {
                ConnectResultEnum ret = ConnectResultEnum.CONNECT_AUTH_ERROR;
                int tryTimes = 0;
                ret = empp.connect(config.hostIP, config.Port, config.Account, config.PassWord);
                while (ret != ConnectResultEnum.CONNECT_OK)
                {
                    tryTimes++;
                    ret = empp.connect(config.hostIP, config.Port, config.Account, config.PassWord);
                }
                if (ret != ConnectResultEnum.CONNECT_OK)
                {
                    _isConnecting = false;
                    throw new Exception("连接登录失败，请检查网络和账号配置。");
                }
            }
            _isConnecting = false;
        }


        /// <summary>
        /// 开始发送消息函数
        /// </summary>
        public bool SendMessages()
        {
            if (message != null && telephone != null)
            {
                try
                {
                    empp.needStatus = true;
                    ShortMessage msg = new ShortMessageClass();
                    Mobiles mobiles = new MobilesClass();
                    foreach (var phone in telephone)
                    {
                        mobiles.Add(phone);
                    }
                    msg.DestMobiles = mobiles;
                    msg.content = message;
                    msg.needStatus = true;
                    msg.ServiceID = config.ServiceID;
                    msg.SendNow = true;
                    msg.srcID = config.Account;
                    CheckConnection();
                    if (empp.connected && empp != null)
                    {
                        try
                        {
                            empp.submit(msg);
                            SubmitResp sp = new SubmitRespClass();
                            SubmitResultEnum s_result = SubmitResultEnum.SUBMIT_ATTIME_ERROR;
                            s_result = sp.Result;
                            if (s_result == SubmitResultEnum.SUBMIT_OK)
                            {
                                return true;
                            }
                            return false;

                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }


                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }



    }
}
