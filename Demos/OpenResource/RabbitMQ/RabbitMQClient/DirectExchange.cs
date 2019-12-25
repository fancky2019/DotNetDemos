using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ.RabbitMQClient
{
    /// <summary>
    /// http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
    /// 路由模式(Direct Exchange)
    ///  direct类型要求routingkey完全相等，
    /// </summary>
    class DirectExchange
    {
        /*
    * 持久化：
    * Exchange：ExchangeDeclare 参数durable: true，宕机只保存Exchange元数据 ，Queue、Message丢失
    * Queue:QueueDeclare 参数durable: true         宕机只保存Queue元数据，Message丢失
    * Message:BasicProperties 属性 Persistent = true;   宕机只保存Queue元数据。
  */
        public const string DeadLetterExchange = "DeadLetterExchange";
        public const string DeadLetterQueue = "DeadLetterQueue";
        public const string DeadLetterRoutingKey = "DeadLetterRoutingKey";
        public void Consumer()
        {
            var exchange = "DirectExchange";
            var routingKey = "DirectExchangeRoutingKey";
            var queue = "DirectExchangeQueue";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "guest", Password = "guest" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //创建死信交换机队列：用于存储死信
                channel.ExchangeDeclare(exchange: DeadLetterExchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: DeadLetterQueue,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                channel.QueueBind(queue: DeadLetterQueue,
                         exchange: DeadLetterExchange,
                         routingKey: DeadLetterRoutingKey);
                #region  设置队列长度
                //Dictionary<string, object> arguments = new Dictionary<string, object>();
                //arguments.Add("x-max-length", 10);
                //channel.QueueDeclare(queue: queue,
                //                     durable: true,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: arguments);
                #endregion


                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                //将队列设置成死信队列，出现死信的情况，加入下面参数指定的队列。
                Dictionary<string, object> arguments = new Dictionary<string, object>();
                ////arguments.Add("x-expires", 30000);
                ////arguments.Add("x-message-ttl", 12000);//队列上消息过期时间，应小于队列过期时间 单位毫秒
                //在RabbitMQ的后台管理创建死信队列和交换机
                // 设置该Queue的死信的信箱
                arguments.Add("x-dead-letter-exchange", DeadLetterExchange);
                // 设置死信routingKey
                arguments.Add("x-dead-letter-routing-key", DeadLetterRoutingKey);
                //arguments.Add("x-message-ttl", 5000);//设置过期时间5s.
                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: arguments);

                channel.QueueBind(queue: queue,
                                  exchange: exchange,
                                  routingKey: routingKey);



                Console.WriteLine(" [*] Waiting for messages.");
                //公平调度：客户端未处理完，不会再给它发送任务
                //参数：提前获取的字节数大小，
                //      提前获取的条数 默认1，参考：https://www.rabbitmq.com/blog/2014/04/14/finding-bottlenecks-with-rabbitmq-3-3/
                //      限定对象，false=限制单个消费者；true=限制整个信道
                channel.BasicQos(0, 30, false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);

                        //制造异常，加入死信队列。也可以设计重试几次不行才加入死信队列
                        //int m = int.Parse("m");

                        //http://localhost:15672/#/queues 可以查看到没有ack，队列没有移除这条信息
                        //Thread.Sleep(30000);
                        channel.BasicAck(ea.DeliveryTag, false);//发送客户端消息任务完成的应答
                    }
                    catch (Exception ex)
                    {
                        //注意：最好不要重新入队，会造成再次分发扔消费不了，又入队的死循环中。应加入死信队列。
                        //方案：使用BasicNack，将消息重新放回队列重新消费
                        //channel.BasicNack(ea.DeliveryTag, false, true);
                        // channel.BasicReject(ea.DeliveryTag, true);
                        //channel.basicNack 与 channel.basicReject 的区别在于basicNack可以拒绝多条消息，而basicReject一次只能拒绝一条消息

                        var basicProperties = ea.BasicProperties;
                        if (basicProperties.Headers != null && basicProperties.Headers.Keys.Contains("x-death"))
                        {
                            var deathDic = basicProperties.Headers["x-death"];
                            //var retryCount = deathDic[0];
                        }
                        //channel.BasicReject(ea.DeliveryTag, true);



                        //java 代码
                        //public long getRetryCount(AMQP.BasicProperties properties)
                        //{
                        //    long retryCount = 0L;
                        //    Map<String, Object> header = properties.getHeaders();
                        //    if (header != null && header.containsKey("x-death"))
                        //    {
                        //        List<Map<String, Object>> deaths = (List<Map<String, Object>>)header.get("x-death");
                        //        if (deaths.size() > 0)
                        //        {
                        //            Map<String, Object> death = deaths.get(0);
                        //            retryCount = (Long)death.get("count");
                        //        }
                        //    }
                        //    return retryCount;
                        //}


                        /*
                         * 死信:
                         * 消息被拒绝（channel.BasicNack或channel.BasicReject）并且requeue=false. 
                          消息TTL过期 
                          队列达到最大长度（队列满了，无法再添加数据到mq中）

                          设置:x-dead-letter-exchange 指定死信送往的交换机 
                          设置:x-dead-letter-routing-key 指定死信的routingkey 
                         */

                        //false:将此消息从队列中销毁，true:重新入队。消息将加入死信队列指定的队列中。
                        //channel.BasicNack(ea.DeliveryTag, false, false);
                        channel.BasicReject(ea.DeliveryTag, false);
                    }



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
