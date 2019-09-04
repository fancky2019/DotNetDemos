using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ.RabbitMQServer
{
    /// <summary>
    /// 官方API:
    /// https://www.rabbitmq.com/dotnet-api-guide.html
    /// 
    /// 路由模式(Direct Exchange)
    /// http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
    /// direct类型要求routingkey完全相等，
    /// </summary>
    class DirectExchange
    {
        /*
         * 持久化：
         * Exchange：ExchangeDeclare 参数durable: true，宕机只保存Exchange元数据 ，Queue、Message丢失
         * Queue:QueueDeclare 参数durable: true         宕机只保存Queue元数据，Message丢失
         * Message:BasicProperties 属性 Persistent = true;   宕机只保存Queue元数据。
       */

        public void Producer()
        {
            var exchange = "DirectExchange";
            var routingKey = "DirectExchangeRoutingKey";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //var guest = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "guest", Password = "guest" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                //避免消息积压，采取流控
                //因为流控而阻塞
                //connection.ConnectionBlocked += HandleBlocked;
                //connection.ConnectionUnblocked += HandleUnblocked;
                ///durable 保存到本地磁盘，下次重启rabbitMQ消息还在
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);


                var message = "DirectExchange:Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                // 将消息标记为持久性。
                var properties = channel.CreateBasicProperties();
                // Sets RabbitMQ.Client.IBasicProperties.DeliveryMode to either persistent (2)  or non-persistent (1).
                //2:持久化，1：不持久化
                properties.Persistent = true;

                //channel.QueueDelete("DirectExchangeQueue");
                //事务：rabbitMQ确认模式的性能优于事务机制。
                //启用确认模式
                channel.ConfirmSelect();
                channel.BasicAcks += (sender, basicAckEventArgs) =>
                {
                    //Name = "RecoveryAwareModel" FullName = "RabbitMQ.Client.Impl.RecoveryAwareModel"
                    var type = sender.GetType();
                    var ch = sender as IModel;
                };

                //没有路由的消息将会回退。
                channel.BasicReturn += (object sender, global::RabbitMQ.Client.Events.BasicReturnEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(body);
                };

                // 当mandatory标志位设置为true时，如果exchange根据自身类型和消息routingKey无法找到一个合适的queue存储消息，
                //那么broker会调用basic.return方法将消息返还给生产者;当mandatory设置为false时，出现上述情况broker会直接将消息丢弃;通俗的讲，
                // mandatory标志告诉broker代理服务器至少将消息route到一个队列中，否则就将消息return给发送者;

                //  mandatory: 默认false。
                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     mandatory: true,
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

