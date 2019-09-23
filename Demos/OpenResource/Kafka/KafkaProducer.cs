using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.Kafka
{
    public class KafkaProducer
    {

        public void Test()
        {
            //ConsoleInput();
            ForInput();
        }

        private void ForInput()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int key = random.Next(100, 200);
                string val = $"message_{key}";
                Producer(key.ToString(), val);
                Thread.Sleep(200);
            }
        }

        private void ConsoleInput()
        {
            var cancelled = false;
            // 取消:Ctrl+C
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cancelled = true;
            };

            while (!cancelled)
            {
                Console.Write("> ");

                string text;
                try
                {
                    //输入message:key和val有个空格
                    text = Console.ReadLine();
                }
                catch (IOException ee)
                {
                    // IO exception is thrown when ConsoleCancelEventArgs.Cancel == true.
                    break;
                }
                if (text == null)
                {
                    // Console returned null before 
                    // the CancelKeyPress was treated
                    break;
                }

                string key = null;
                string val = text;

                // split line if both key and value specified.
                int index = text.IndexOf(" ");
                if (index != -1)
                {
                    key = text.Substring(0, index);
                    val = text.Substring(index + 1);
                }

                Producer(key, val);
            }
        }

        public async void Producer(string key, string val)
        {
            string brokerList = "localhost:9092";
            string topicName = "topicName";
            var config = new ProducerConfig { BootstrapServers = brokerList };

            //生产者确认生产成功。
            /*
             acks=0: producer 不等待 Leader 确认，只管发出即可；最可能丢失消息，适用于高吞吐可丢失的业务；
             acks=1(默认值): producer 等待 Leader 写入本地日志后就确认；之后 Leader 向 Followers 同步时，如果 Leader 宕机会导致消息没同步而丢失，producer 却依旧认为成功；
             acks=all/-1: producer 等待 Leader 写入本地日志、而且 Leader 向 Followers 同步完成后才会确认；最可靠。
             */
            //config.Acks = Acks.All;
            using (var producer = new ProducerBuilder<string, string>(config)
                  // 如果不指定序列化类型，Confluent.Kafka 内部Producer<TKey, TValue>类的字典成员defaultSerializers
                  /*
                    private static readonly Dictionary<Type, object> defaultSerializers = new Dictionary<Type, object>
                    {
                        { typeof(Null), Serializers.Null },
                        { typeof(int), Serializers.Int32 },
                        { typeof(long), Serializers.Int64 },
                        { typeof(string), Serializers.Utf8 },
                        { typeof(float), Serializers.Single },
                        { typeof(double), Serializers.Double },
                        { typeof(byte[]), Serializers.ByteArray }
                    };
                   */
                  // 会根据key 类型反射获取对应的序列化类型
                  .SetValueSerializer(Serializers.Utf8)
                  .SetKeySerializer(Serializers.Utf8)//Deserializers
                  .Build())
            {
                try
                {
                    //  Messages 中Key 决定消息的partion,内部hash(key)，如果不指定Key将随机指定分区（partion）

                    // Note: Awaiting the asynchronous produce request below prevents flow of execution
                    // from proceeding until the acknowledgement from the broker is received (at the 
                    // expense of low throughput
                    var deliveryReport = await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = val });
                    Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");


                    //实际生产环境使用不阻塞的方法，不使用async，上面是官方的Demo ，测试用
                    //producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = val })
                    //    .ContinueWith(task =>
                    //    {
                    //        // task.Result.Status
                    //        //task.Result.Offset
                    //        Console.WriteLine($"delivered to: {task.Result.TopicPartitionOffset}");
                    //    });

                    //同步方法
                    // DeliveryReport<TKey, TValue> : DeliveryResult<TKey, TValue>
                    // DeliveryReport: Produce生产成功的参数回调，执行结果的信息。
                    //producer.Produce(topicName, new Message<string, string> { Key = key, Value = val }, deliveryReport =>
                    //{
                    //});
                    //producer.Flush(TimeSpan.FromSeconds(10));
                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
            }
        }

        public async void Producer()
        {
            string brokerList = "localhost:9092";
            string topicName = "topicName";

            var config = new ProducerConfig { BootstrapServers = brokerList };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {

                var cancelled = false;
                // 取消:Ctrl+C
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };

                while (!cancelled)
                {
                    Console.Write("> ");

                    string text;
                    try
                    {
                        //输入message:key和val有个空格
                        text = Console.ReadLine();
                    }
                    catch (IOException ee)
                    {
                        // IO exception is thrown when ConsoleCancelEventArgs.Cancel == true.
                        break;
                    }
                    if (text == null)
                    {
                        // Console returned null before 
                        // the CancelKeyPress was treated
                        break;
                    }

                    string key = null;
                    string val = text;

                    // split line if both key and value specified.
                    int index = text.IndexOf(" ");
                    if (index != -1)
                    {
                        key = text.Substring(0, index);
                        val = text.Substring(index + 1);
                    }

                    try
                    {
                        // Note: Awaiting the asynchronous produce request below prevents flow of execution
                        // from proceeding until the acknowledgement from the broker is received (at the 
                        // expense of low throughput).
                        var deliveryReport = await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = val });

                        Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");


                        //        //// 3后 Flush到磁盘
                        //        //producer.Flush(TimeSpan.FromSeconds(3));
                    }
                    catch (ProduceException<string, string> e)
                    {
                        Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                    }
                }


            }
        }

    }
}
