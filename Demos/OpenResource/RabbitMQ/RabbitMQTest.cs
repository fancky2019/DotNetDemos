using Demos.Demos2018.RabbitMQ.RabbitMQServer;
using Demos.OpenResource.RabbitMQ.RabbitMQClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.RabbitMQ
{
    /// <summary>
    /// 官方API:
    /// https://www.rabbitmq.com/dotnet-api-guide.html
    /// </summary>
    class RabbitMQTest
    {
        public void  Test()
        {
    

            Task.Run(() =>
            {
                //DEMO  链接：http://www.rabbitmq.com/getstarted.html
                //NuGet添加RabbitMQ.Client引用
                //RabbitMQ UI管理:http://localhost:15672/   账号:guest 密码:guest
                //先启动订阅，然后启动发布
                //var factory = new ConnectionFactory(){ HostName = "192.168.1.121", Port = 5672 }; //HostName = "localhost",
                //用下面的实例化，不然报 None of the specified endpoints were reachable
                //var factory = new ConnectionFactory() { HostName = "192.168.1.121", Port = 5672, UserName = "fancky", Password = "123456" };

                //http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html
                //NuGet安装RabbitMQ.Client

                //new Demos.Demos2018.RabbitMQ.RabbitMQClient.FanoutExchange().Consumer();
                //new Demos.Demos2018.RabbitMQ.RabbitMQClient.DirectExchange().Consumer();
                //new Demos.Demos2018.RabbitMQ.RabbitMQClient.TopicExchange().Consumer();

                new DelayRetryConsumer().Consumer();
            });

            Thread.Sleep(1000);
            //生产者没有创建队列。
            Task.Run(() =>
            {
                //DEMO  链接：http://www.rabbitmq.com/getstarted.html
                //NuGet添加RabbitMQ.Client引用
                //RabbitMQ UI管理:http://localhost:15672/   账号:guest 密码:guest
                //先启动订阅，然后启动发布
                //var factory = new ConnectionFactory(){ HostName = "192.168.1.121", Port = 5672 }; //HostName = "localhost",
                //用下面的实例化，不然报 None of the specified endpoints were reachable
                //var factory = new ConnectionFactory() { HostName = "192.168.1.121", Port = 5672, UserName = "fancky", Password = "123456" };

                //http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html
                //NuGet安装RabbitMQ.Client
                //new FanoutExchange().Producer();
                new DirectExchange().Producer();
                //new TopicExchange().Producer();
            });
 
        }
    }
}
