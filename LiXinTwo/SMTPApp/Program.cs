using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Common;

namespace SMTPApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("邮件服务队列订阅中...（请勿关闭）");
                RabbitMQHelper.Instance.SendMessage(null);
                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {
                    RabbitMQHelper.Instance.ReceivingMessagesPassive(new Email().SendAllEmail);
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Read();
                throw;
            }
            Console.ReadLine();

        }
    }
}
