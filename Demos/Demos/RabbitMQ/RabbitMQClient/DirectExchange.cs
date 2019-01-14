using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos.RabbitMQ.RabbitMQClient
{
    /// <summary>
    /// http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
    /// 路由模式(Direct Exchange)
    ///  direct类型要求routingkey完全相等，
    /// </summary>
    class DirectExchange
    {
        public void Consumer()
        {
            var exchange = "DirectExchange";
            var routingKey = "DirectExchangeRoutingKey";
            var queue = "DirectExchangeQueue";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
         
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                #region  设置队列长度
                //Dictionary<string, object> arguments = new Dictionary<string, object>();
                //arguments.Add("x-max-length", 10);
                //channel.QueueDeclare(queue: queue,
                //                     durable: true,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: arguments);
                #endregion

                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: queue,
                                  exchange: exchange,
                                  routingKey: routingKey);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received '{0}':'{1}'",routingKey, message);

                    //http://localhost:15672/#/queues 可以查看到没有ack，队列没有移除这条信息
                    Thread.Sleep(30000);
                    channel.BasicAck(ea.DeliveryTag, false);//发送客户端消息任务完成的应答
               
           
                };
                channel.BasicConsume(queue: queue,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }
}
