using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ.RabbitMQServer
{
    /// <summary>
    /// http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html
    /// 订阅模式(Fanout Exchange)
    /// </summary>
    class FanoutExchange
    {
        public void Producer()
        {
            var exchange = "FanoutExchange";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                var props = channel.CreateBasicProperties();
                //持久化
                props.Persistent = true;
                var message = "FanoutExchange";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchange,
                                     routingKey: "",
                                     basicProperties: props,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }


    }

}
