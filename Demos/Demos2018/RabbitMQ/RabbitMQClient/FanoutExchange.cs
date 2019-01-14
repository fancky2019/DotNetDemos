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
    /// http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html
    /// 订阅模式(Fanout Exchange)Publish/Subscribe
    /// </summary>
    class FanoutExchange
    {
        public void Consumer()
        {
            var exchange = "FanoutExchange";
            var queue = "FanoutExchangeQueue";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

                channel.QueueDeclare(queue: queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                channel.QueueBind(queue: queue,
                                  exchange: exchange,
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x]  Received: {0}", message);
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
