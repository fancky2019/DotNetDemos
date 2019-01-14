using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ.RabbitMQServer
{
    /// <summary>
    /// http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
    /// 路由模式(Direct Exchange)
    /// direct类型要求routingkey完全相等，
    /// </summary>
    class DirectExchange
    {
        public void Producer()
        {
            var exchange = "DirectExchange";
            var routingKey = "DirectExchangeRoutingKey";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                //公平调度：客户端未处理完，不会再给它发送任务
                channel.BasicQos(0, 1, false);
                var message = "DirectExchange:Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                // 将消息标记为持久性。
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                //channel.QueueDelete("DirectExchangeQueue");
                //启用确认模式
                channel.ConfirmSelect();
                channel.BasicAcks += (sender, basicAckEventArgs) =>
                {
                    //Name = "RecoveryAwareModel" FullName = "RabbitMQ.Client.Impl.RecoveryAwareModel"
                    var type = sender.GetType();
                    var ch = sender as IModel;
                };


                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     basicProperties: properties,
                                     body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);


                var confirm = channel.WaitForConfirms();
                bool b = confirm;


                //分布式事务 性能不好，用上面确认模式
                //try
                //{
                //    channel.TxSelect();
                //    channel.BasicPublish(exchange: exchange,
                //                         routingKey: routingKey,
                //                         basicProperties: properties,
                //                         body: body);
                //}
                //catch(Exception ex)
                //{
                //    channel.TxRollback();
                //}

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

    
    }
}

