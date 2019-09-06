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
        public void Test()
        {
            new KafkaProducer().Producer();
            new KafkaConsumer().Test();

            int m = 0;

        }
    }
}
