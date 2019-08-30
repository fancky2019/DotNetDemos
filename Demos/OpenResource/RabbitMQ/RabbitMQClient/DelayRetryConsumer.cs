using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.RabbitMQ.RabbitMQClient
{
    /// <summary>
    /// RabbitMQ重试：工作队列设置通过死信进入重试队列，在重试队列设置TTL（达到延迟目的）进入工作队列达到重试目的。
    /// 
    ///  死信:
    ///       消息被拒绝（channel.BasicNack或channel.BasicReject）并且requeue=false. 
    ///       消息TTL过期
    ///       队列达到最大长度（队列满了，无法再添加数据到mq中）
    ///  
    ///       设置:x-dead-letter-exchange 指定死信送往的交换机
    ///       设置:x-dead-letter-routing-key 指定死信的routingkey
    /// </summary>
    public class DelayRetryConsumer
    {

        public void Consumer()
        {
            var exchange = "DirectExchange";
            var queue = "DirectExchangeQueue";
            var routingKey = "DirectExchangeRoutingKey";

            //重试队列
            string retryExchange = "RetryExchange";
            string retryQueue = "RetryQueue";
            string retryKey = "RetryKey";

            //失败队列，如果重试3次仍然失败就加入此队列。
            string failedExchange = "FailedExchange";
            string failedQueue = "FailedQueue";
            string failedKey = "FailedKey";

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "guest", Password = "guest" };
            var factory = new ConnectionFactory() { HostName = "192.168.1.105", Port = 5672, UserName = "fancky", Password = "123456" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                //创建重试交换机、重试队列
                channel.ExchangeDeclare(exchange: retryExchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                //将队列设置成死信队列，出现死信的情况，加入下面参数指定的队列。
                Dictionary<string, object> retryArguments = new Dictionary<string, object>();
                ////arguments.Add("x-expires", 30000);
                //arguments.Add("x-max-length", 10);
                retryArguments.Add("x-message-ttl", 10000);//队列上消息过期时间，应小于队列过期时间 单位毫秒
                //在RabbitMQ的后台管理创建死信队列和交换机
                // 设置该Queue的死信的信箱
                retryArguments.Add("x-dead-letter-exchange", exchange);
                // 设置死信routingKey
                retryArguments.Add("x-dead-letter-routing-key", routingKey);
                channel.QueueDeclare(queue: retryQueue,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: retryArguments);

                channel.QueueBind(queue: retryQueue,
                         exchange: retryExchange,
                         routingKey: retryKey);



                //失败交换机、队列：重试仍然失败的队列
                channel.ExchangeDeclare(exchange: failedExchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: failedQueue,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                channel.QueueBind(queue: failedQueue,
                                 exchange: failedExchange,
                                 routingKey: failedKey);



                //消费者交换机队列：
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                //将队列设置成死信队列，出现死信的情况，加入下面参数指定的队列。
                Dictionary<string, object> arguments = new Dictionary<string, object>();

                ////arguments.Add("x-expires", 30000);
                ////arguments.Add("x-message-ttl", 12000);//队列上消息过期时间，应小于队列过期时间 单位毫秒
                //在RabbitMQ的后台管理创建死信队列和交换机
                // 设置该Queue的死信的信箱
                arguments.Add("x-dead-letter-exchange", retryExchange);
                // 设置死信routingKey
                arguments.Add("x-dead-letter-routing-key", retryKey);
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
                        int m = int.Parse("m");

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


                        //没有加入死信队列basicProperties.Headers=null
                        var basicProperties = ea.BasicProperties;
                        if (basicProperties.Headers != null && basicProperties.Headers.Keys.Contains("x-death"))
                        {
                            //List<object>
                            var deathListObject = (List<object>)basicProperties.Headers["x-death"];

                            #region  header
                            ///*
                            // * [0] {[count,1]}
                            // * [1] {[exchange,{byte[]}]}
                            // * [2] {[queue,{byte[]}]}
                            // * [3] {[reason,{byte[]}]}
                            // * [4] {[routing-keys,Count=1]}
                            // * [5] {[time,{((time_t)1567129245)}]}]}//时间戳
                            // */
                            //var headerOne = (Dictionary<string, object>)deathListObject[0];
                            ////RetryExchange
                            //string exchange = Encoding.UTF8.GetString((byte[])headerOne["exchange"]);
                            ////RetryQueue
                            //var queue = Encoding.UTF8.GetString((byte[])headerOne["queue"]);
                            ////expired
                            //var reason = Encoding.UTF8.GetString((byte[])headerOne["reason"]);
                            ////RetryKey
                            //var routing_keys = Encoding.UTF8.GetString((byte[])((List<object>)headerOne["routing-keys"])[0]);
                            #endregion

                            //Dictionary<string, object>
                            var deathDicStrObj = (Dictionary<string, object>)deathListObject[0];


                            var retryCount = int.Parse(deathDicStrObj["count"].ToString());
                            //此处是重试队列的3次加上消费1次共计执行消费4次和Springboot配置重试几次就共计执行几次多了一次。
                            //重试3次还不成功，加入失败的队列。
                            if (retryCount == 3)
                            {
                                //把消息发送到失败队列同时Ack掉此条消息
                                //发送到失败队列
                                var properties = channel.CreateBasicProperties();
                                properties.Persistent = true;
                                channel.BasicPublish(exchange: failedExchange,
                                                       routingKey: failedKey,
                                                       basicProperties: properties,
                                                       body: ea.Body);
                                //Ack掉
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                            else
                            {
                                //重试不足3次，继续拒绝（死信）以加入重试队列。
                                channel.BasicReject(ea.DeliveryTag, false);
                            }
                        }
                        else
                        {
                            //没有重试，拒绝（死信）以加入重试队列。
                            channel.BasicReject(ea.DeliveryTag, false);
                        }

                        //重试队列设置了10s过期，为了从rabbitMQ管理中心的网页队列列表中看到
                        //数据在重试，消费队列中切换，此处延迟15s。
                        Thread.Sleep(15000);
                        //channel.BasicReject(ea.DeliveryTag, true);

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
