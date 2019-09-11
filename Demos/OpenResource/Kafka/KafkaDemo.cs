using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Kafka
{
    /// <summary>
    /// NuGet:Confluent.Kafka
    /// https://github.com/confluentinc/confluent-kafka-dotnet/
    /// </summary>
    public class KafkaDemo
    {
        public async void Test()
        {
            //消费者单开一个程序，将消费者的代码拷贝出去执行。
            new KafkaProducer().Test();
            int m = 0;
            Console.WriteLine("finshed");
            Console.ReadLine();

        }
    }
}
