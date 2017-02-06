using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RabbitMQ.Common
{
    public abstract class RabbitMQBase
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        private static readonly RabbitMQConfigSection ConfigSection =
            (RabbitMQConfigSection)ConfigurationManager.GetSection("RabbitMQ");

        /// <summary>
        /// RabbitMQ所在服务器
        /// </summary>
        protected static string BaseServerAddress
        {
            get
            {
                return ConfigSection.KeyValues["serverAddress"].Value;
            }
        }

        protected static string BaseUserName
        {
            get
            {
                return ConfigSection.KeyValues["userName"].Value;
            }
        }

        protected static string BasePassword
        {
            get
            {
                return ConfigSection.KeyValues["password"].Value;
            }
        }

        /// <summary>
        /// 虚拟机名
        /// </summary>
        protected static string BaseVirtualHost
        {
            get
            {
                return //ConfigurationManager.AppSettings["virtualHost"];
                //修改之前从Section中读取的内容 ByRichter2012-11-19 14:44:17
                ConfigSection.KeyValues["virtualHost"].Value;
            }
        }

        /// <summary>
        /// 路由
        /// </summary>
        protected static string BaseExchange
        {
            get
            {
                return ConfigSection.KeyValues["exchange"].Value;
            }
        }

        /// <summary>
        /// 路由关键字
        /// </summary>
        protected static string BaseRoutingKey
        {
            get
            {
                return ConfigSection.KeyValues["routingKey"].Value;
            }
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        protected static string BaseQueue
        {
            get
            {
                return ConfigSection.KeyValues["queue"].Value;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息实体</param>
        public abstract void SendMessage(object message);

        /// <summary>
        /// 接收到消息后的处理
        /// </summary>
        /// <param name="message"></param>
        public delegate void MessageHandler(object message);

        /// <summary>
        /// 主动接收消息
        /// </summary>
        /// <param name="handler">接收到消息后的处理</param>
        public abstract void ReceivingMessagesActive(MessageHandler handler);

        /// <summary>
        /// 订阅模式接收消息
        /// </summary>
        /// <param name="handler">接收到消息后的处理</param>
        public abstract void ReceivingMessagesPassive(MessageHandler handler);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">要序列化的对象</param>
        /// <returns>返回存放序列化后的数据缓冲区</returns>
        protected static byte[] Serialize(object data)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">数据缓冲区</param>
        /// <returns>对象</returns>
        protected static object Deserialize(byte[] data)
        {
            var formatter = new BinaryFormatter();
            var rems = new MemoryStream(data);
            return formatter.Deserialize(rems);
        }
    }
}
