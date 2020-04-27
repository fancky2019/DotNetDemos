using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
    class DirectExchangeProducer
    {
        /*
         * 持久化：
         * Exchange：ExchangeDeclare 参数durable: true，宕机只保存Exchange元数据 ，Queue、Message丢失
         * Queue:QueueDeclare 参数durable: true         宕机只保存Queue元数据，Message丢失
         * Message:BasicProperties 属性 Persistent = true;   宕机只保存Queue元数据。
         * 
         * 信道：TCP连接的复用，一个TCP连接可以有多个channel。
         * 消息生产：
         * 消息发布到交换机同时指定路由key，交换机根据路由key，将消息存储到指定队列上。
         * 队列：队列和交换机、路由key绑定在一起。
         * 消费：
         * 消费指定队列的数据。
         * 
       */

        public void ProduceIndividually(int i=0)
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

                //生产端声明队列，避免消息因队列不存在而无法投递。
                //当队列存在不会重复创建
                //var queue = "DirectExchangeQueue";
                //channel.QueueDeclare(queue: queue,
                //              durable: true,
                //              exclusive: false,
                //              autoDelete: false,
                //              arguments: null);

                //channel.QueueBind(queue: queue,
                //                  exchange: exchange,
                //                  routingKey: routingKey);



                var message = $"Message-{i} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}- DirectExchange:Hello World!";
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



                //在rabbitMQ后台管理（http://localhost:15672/#/queues）删除DirectExchange队列，消息无法投递到队列将进入此回调
                //没有路由的消息： channel.BasicPublish方法参数设置 mandatory: true,
                //没有路由的消息将会回退,消息没有找到可路由转发的队列，立即回发给生产者。
                channel.BasicReturn += (object sender, global::RabbitMQ.Client.Events.BasicReturnEventArgs e) =>
                {

                    var message = Encoding.UTF8.GetString(e.Body);
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

                try
                {
                    //官网采用的方法，
                    //F12显示：超时将抛OperationInterruptedException异常
                    //实际超时抛IOException异常，注释可能有问题。
                    channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");
                }
                catch (OperationInterruptedException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");

                }

                //var confirm = channel.WaitForConfirms();
                //bool b = confirm;


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

        }

        public void ProduceInBatches()
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

                //channel.QueueDelete("DirectExchangeQueue");
                //事务：rabbitMQ确认模式的性能优于事务机制。
                //启用确认模式
                channel.ConfirmSelect();



                //没有路由的消息将会回退,消息没有找到可路由转发的队里，立即回发给生产者。
                channel.BasicReturn += (object sender, global::RabbitMQ.Client.Events.BasicReturnEventArgs e) =>
                {

                    var message = Encoding.UTF8.GetString(e.Body);
                };


                // 将消息标记为持久性。
                var properties = channel.CreateBasicProperties();
                // Sets RabbitMQ.Client.IBasicProperties.DeliveryMode to either persistent (2)  or non-persistent (1).
                //2:持久化，1：不持久化
                properties.Persistent = true;

                for (int i = 0; i < 100; i++)
                {
                    var message = $"Message{i}:DirectExchange:Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
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
                }
                try
                {
                    //官网采用的方法，
                    //F12显示：超时将抛OperationInterruptedException异常
                    //实际超时抛IOException异常，注释可能有问题。
                    channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");
                }
                catch (OperationInterruptedException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");

                }
                //var confirm = channel.WaitForConfirms();
                //bool b = confirm;


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

        }

        /// <summary>
        /// 为完成的确认消息
        /// 生产环境中可以放在Redis中。
        /// </summary>
        private ConcurrentDictionary<ulong, string> _outstandingConfirms = new ConcurrentDictionary<ulong, string>();

        public void ProduceInBatchesAsync()
        {
            var exchange = "DirectExchange";
            var routingKey = "DirectExchangeRoutingKey";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //var guest = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "guest", Password = "guest" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //RabbitMQ服务端的资源配置（内存、磁盘），服务器使用达到配置而产生警告，进而产生流控。
               // RabbitMQ服务器资源（磁盘、内存）达到服务端配置而产生警报造成流控。
                //避免消息积压，采取流控
                //因为流控而阻塞：提高消费能力解决消息积压，而不是采用流控。
                connection.ConnectionBlocked += (sender, connectionBlockedEventArgs) =>
                 {
                     Console.WriteLine(connectionBlockedEventArgs.Reason);
                 };
                connection.ConnectionUnblocked += (sender, eventArgs) =>
                {
                };
                ///durable 保存到本地磁盘，下次重启rabbitMQ消息还在
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);




                //channel.QueueDelete("DirectExchangeQueue");
                //事务：rabbitMQ确认模式的性能优于事务机制。
                //启用确认模式
                channel.ConfirmSelect();


                //没有路由的消息将会回退,消息没有找到可路由转发的队里，立即回发给生产者。
                channel.BasicReturn += (object sender, global::RabbitMQ.Client.Events.BasicReturnEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body);
                };


                //服务端确认回调：获取不到确认的消息
                channel.BasicAcks += (sender, ea) =>
                {
                    // code when message is confirmed
                    var type = sender.GetType();
                    var ch = sender as IModel;

                    var publishSeqNo = ea.DeliveryTag;
                    var multiple = ea.Multiple;

                    //如果是批量确认，从内存中移除小于该发布序列号的数据。channel.NextPublishSeqNo：自增的UInt64
                    //Broker返回的Multiple可能是false或true
                    if (ea.Multiple)
                    {
                        var confirmed = _outstandingConfirms.Where(k => k.Key <= ea.DeliveryTag);
                        foreach (var entry in confirmed)
                            _outstandingConfirms.TryRemove(entry.Key, out _);
                    }
                    else
                    {
                        _outstandingConfirms.TryRemove(ea.DeliveryTag, out _);
                    }

                };

                //服务端没有确认的消息
                channel.BasicNacks += (sender, ea) =>
                {

                    //code when message is nack-ed
                    _outstandingConfirms.TryGetValue(ea.DeliveryTag, out string body);
                    Console.WriteLine($"Message with body {body} has been nack-ed. Sequence number: {ea.DeliveryTag}, multiple: {ea.Multiple}");
                    //日志记录单独处理确认失败的消息，一般不会确认失败。
                };


                // 将消息标记为持久性。
                var properties = channel.CreateBasicProperties();
                // Sets RabbitMQ.Client.IBasicProperties.DeliveryMode to either persistent (2)  or non-persistent (1).
                //2:持久化，1：不持久化
                properties.Persistent = true;


                for (int i = 0; i < 100; i++)
                {
                    var message = $"Message{i}:DirectExchange:Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    // 当mandatory标志位设置为true时，如果exchange根据自身类型和消息routingKey无法找到一个合适的queue存储消息，
                    //那么broker会调用basic.return方法将消息返还给生产者;当mandatory设置为false时，出现上述情况broker会直接将消息丢弃;通俗的讲，
                    // mandatory标志告诉broker代理服务器至少将消息route到一个队列中，否则就将消息return给发送者;

                    //channel.NextPublishSeqNo：自增的UInt64
                    _outstandingConfirms.TryAdd(channel.NextPublishSeqNo, message);
                    //  mandatory: 默认false。
                    channel.BasicPublish(exchange: exchange,
                                         routingKey: routingKey,
                                         mandatory: true,
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }

                try
                {
                    //官网采用的方法，
                    //F12显示：超时将抛OperationInterruptedException异常
                    //实际超时抛IOException异常，注释可能有问题。
                    channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");
                }
                catch (OperationInterruptedException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine($"生产失败！");

                }

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
                Console.ReadLine();
            }

        }


    }
}

