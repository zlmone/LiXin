using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQClient.ConfigSection;

namespace SendMessageOCX
{
    public class AccountBase
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        private static readonly RabbitMQConfigSection ConfigSection =
            (RabbitMQConfigSection)ConfigurationManager.GetSection("Account");

        /// <summary>
        /// ip地址
        /// </summary>
        public static string Host
        {
            get
            {
                return ConfigSection.KeyValues["host"].Value;
            }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public static string port
        {
            get
            {
                return ConfigSection.KeyValues["port"].Value;
            }
        }


        /// <summary>
        /// 用户名
        /// </summary>
        public static string AccountId
        {
            get
            {
                return ConfigSection.KeyValues["accountId"].Value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get
            {
                return ConfigSection.KeyValues["password"].Value;
            }
        }


        /// <summary>
        /// 服务代号
        /// </summary>
        public static string ServiceId
        {
            get
            {
                return ConfigSection.KeyValues["serviceId"].Value;
            }
        }


    }
}
