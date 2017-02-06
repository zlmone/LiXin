using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    [Serializable]
    public class SmsMessage
    {
        /// <summary>
        /// 短信内容
        /// </summary>
        public string message
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号码集合
        /// </summary>
        public List<string> telePhoneList
        {
            get;
            set;
        }

        /// <summary>
        /// 配置，此处读的是缓存。。。厉害吧
        /// </summary>
        public SMSConfig config
        {
            get;
            set;
        }

        public List<KeyValuePair<string, string>> list
        {
            get;
            set;
        }

    }
}
