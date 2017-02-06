using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Common
{
    public class RabbitMQHelper : RabbitMQBase
    {
        /// <summary>
        /// RabbitMQ所在服务器
        /// </summary>
        private string ServerAddress
        {
            get;
            set;
        }

        private string UserName
        {
            get;
            set;
        }

        private string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 虚拟机名
        /// </summary>
        private string VirtualHost
        {
            get;
            set;
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        private string Queue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverAddress">RabbitMQ所在服务器</param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="virtualHost">虚拟机名</param>
        /// <param name="queue">队列名称</param>
        public RabbitMQHelper(string serverAddress, string userName, string password, string virtualHost, string queue)
        {
            ServerAddress = serverAddress;
            UserName = userName;
            Password = password;
            VirtualHost = virtualHost;
            Queue = queue;
        }

        private static RabbitMQHelper _instance;

        public static RabbitMQHelper Instance
        {
            get
            {
                return _instance ??
                       (_instance =
                        new RabbitMQHelper(BaseServerAddress, BaseUserName, BasePassword, BaseVirtualHost, BaseQueue));
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息实体</param>
        public override void SendMessage(object message)
        {
            //服务器所在的主机ip   
            var uri = new Uri(ServerAddress);

            //创建连接
            var cf = new ConnectionFactory
                         {
                             UserName = UserName,
                             Password = Password,
                             VirtualHost = VirtualHost,
                             RequestedHeartbeat = 0,
                             Endpoint = new AmqpTcpEndpoint(uri)
                         };

            //创建一个连接到具体总结点的连接   
            using (var conn = cf.CreateConnection())
            {
                //创建并返回一个新连接到具体节点的通道   
                using (var ch = conn.CreateModel())
                {
                    //声明一个路由   
                    ch.ExchangeDeclare(BaseExchange, ExchangeType.Direct);
                    //声明一个队列   
                    ch.QueueDeclare(Queue, true, false, false, null);
                    //将一个队列和一个路由绑定起来。并制定路由关键字     
                    ch.QueueBind(Queue, BaseExchange, BaseRoutingKey);
                    //发送信息
                    ch.BasicPublish(BaseExchange, BaseRoutingKey, null, message != null ? Serialize(message) : null);
                }
            }
        }

        /// <summary>
        /// 主动接收消息
        /// </summary>
        /// <param name="handler">接收到消息后的处理</param>
        public override void ReceivingMessagesActive(MessageHandler handler)
        {
            var cf = new ConnectionFactory
                         {
                             Uri = ServerAddress,
                             UserName = UserName,
                             Password = Password,
                             VirtualHost = VirtualHost,
                             RequestedHeartbeat = 0
                         };

            using (var conn = cf.CreateConnection())
            {
                using (var ch = conn.CreateModel())
                {
                    var i = 1;

                    //普通使用方式BasicGet   
                    //noAck = true，不需要回复，接收到消息后，queue上的消息就会清除   
                    //noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，   
                    //直到调用channel.basicAck(deliveryTag, false);    
                    //queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息   
                    while (true)
                    {
                        var res = ch.BasicGet(Queue, false /*noAck*/);
                        if (res == null)
                        {
                            Console.WriteLine("已经没有消息了！");
                            break;
                        }

                        if (res.Body.Length != 0)
                        {
                            Console.Write(i++ + "   ");
                            if (handler != null)
                            {
                                handler(Deserialize(res.Body));
                            }

                            ch.BasicAck(res.DeliveryTag, false);
                        }
                        else
                        {
                            ch.BasicAck(res.DeliveryTag, false);
                            continue;
                        }

                        Console.WriteLine();
                    }
                    ch.Close();
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 订阅模式接收消息
        /// </summary>
        /// <param name="handler">接收到消息后的处理</param>
        public override void ReceivingMessagesPassive(MessageHandler handler)
        {
            var cf = new ConnectionFactory
                         {
                             Uri = ServerAddress,
                             UserName = UserName,
                             Password = Password,
                             VirtualHost = VirtualHost,
                             RequestedHeartbeat = 0
                         };

            using (var conn = cf.CreateConnection())
            {
                using (var ch = conn.CreateModel())
                {
                    var consumer = new QueueingBasicConsumer(ch);
                    var i = 1;

                    //noAck = true，不需要回复，接收到消息后，queue上的消息就会清除   
                    //noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，   
                    //直到调用channel.basicAck(deliveryTag, false);    
                    //queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息   
                    ch.BasicConsume(Queue, false /*noAck*/, consumer);
                    while (true)
                    {
                        try
                        {
                            var res = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                            if (res.Body.Length != 0)
                            {
                                Console.Write(i++ + "   ");
                                if (handler != null)
                                {
                                    handler(Deserialize(res.Body));
                                }

                                ch.BasicAck(res.DeliveryTag, false);
                            }
                            else
                            {
                                ch.BasicAck(res.DeliveryTag, false);
                                continue;
                            }

                            Console.WriteLine();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            throw;
                        }
                    }
                }
            }
        }
    }
}
