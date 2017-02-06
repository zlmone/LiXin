using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Messaging;

namespace InvincibleCommon
{
    public class MSMQManager
    {
        //".\\private$\\myQueue"
        private static string MSMQ = ".\\private$\\" + ConfigurationManager.AppSettings["MSMQ"];
        public static void SendMessage(object msg)
        {
            try
            {
                //连接到本地的队列
                MessageQueue myQueue = new MessageQueue(MSMQ);
                Message myMessage = new Message();
                myMessage.Body = msg;
                myMessage.Formatter = new XmlMessageFormatter(new[] { typeof(FileMSMQ) });
                //发送消息到队列中
                myQueue.Send(myMessage);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReceiveMessage()
        {
            MessageQueue myQueue = new MessageQueue(MSMQ);
            Message[] message = myQueue.GetAllMessages();
            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            for (int i = 0; i < message.Length; i++)
            {
                message[i].Formatter = formatter;
                Console.WriteLine(message[i].Body.ToString());
            }
        }
    }

    [Serializable]
    public class FileMSMQ
    {
        public string fileName
        {
            get;
            set;
        }
        public string oldName
        {
            get;
            set;
        }
    }
}
