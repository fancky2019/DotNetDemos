using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ.RabbitMQClient
{
    /// <summary>
    /// http://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html
    /// 通配符模式（Topic Exchange）
    /// 路由键支持模糊匹配，符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词
    /// </summary>
    class TopicExchange
    {
        public void Consumer()
        {
            var exchange = "TopicExchange";
            // receive all the logs:"#"
            // receive all logs from the facility "kern.*":
            //"*.critical"
            //"kern.*" "*.critical"
            //var bindingKey = "#";
            //var bindingKey = "TopicExchange.#";
            var routingKey = "TopicExchangeRoutingKey.*";
            //var routingKey = "TopicExchangeRoutingKey.#";
            var queue = "TopicExchangeQueue";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                channel.QueueDeclare(queue: queue,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

                channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);
     

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey,message);
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
