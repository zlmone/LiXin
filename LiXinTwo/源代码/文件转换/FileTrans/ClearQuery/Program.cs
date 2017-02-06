using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            //发送空消息，确认服务器
            RabbitMQClient.RabbitMQHelper.Instance.SendMessage(null);
            //rabbitmq订阅式接收消息并转换
            RabbitMQClient.RabbitMQHelper.Instance.ReceivingMessagesActive(null);

            Console.WriteLine("OK");
            Console.Read();
        }
    }
}
