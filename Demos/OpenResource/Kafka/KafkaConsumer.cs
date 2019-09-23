using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.Kafka
{
    /// <summary>
    /// 
    /// </summary>
    class KafkaConsumer
    {
        /*
         *  Kafka 每个topic下有多个partion,每个partion有一个leader多个follower，
         *  Kafka 采用leader读写，follower备份，leader宕机，从zookeeper从follower中选举一个leader.
         *        producer往partion的leader写，follower发起同步。
         *        comsumer从partion的leader读
         *        
         *  
         *  
         *  
         *  
         *  Earliest 
            当各分区下有已提交的offset时，从提交的offset开始消费；无提交的offset时，从头开始消费 
            Latest 
            当各分区下有已提交的offset时，从提交的offset开始消费；无提交的offset时，消费新产生的该分区下的数据 
            Error 
            topic各分区都存在已提交的offset时，从offset后开始消费；只要有一个分区不存在已提交的offset，则抛出异常
         */
        /// <summary>
        ///     In this example
        ///         - offsets are manually committed.
        ///         - no extra thread is created for the Poll (Consume) loop.
        /// </summary>
        public static void Run_Consume(string brokerList, List<string> topics, CancellationToken cancellationToken)
        {
     
            var dic = new Dictionary<string, string>();
            dic.Add("max.poll.records", "2");//暂时不支持此配置选项。
            //一个线程一个消费者
            var config = new ConsumerConfig()
            {
                BootstrapServers = brokerList,
                GroupId = "csharp-consumer",
                EnableAutoCommit = false,// 设置非自动偏移，业务逻辑完成后手动处理偏移，防止数据丢失
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true      
            };

            const int commitPeriod = 5;

            // Note: If a key or value deserializer is not set (as is the case below), the 
            // deserializer corresponding to the appropriate type from Confluent.Kafka.Deserializers
            // will be used automatically (where available). The default deserializer for string
            // is UTF8. The default deserializer for Ignore returns null for all input data
            // (including non-null data).
            //using (var consumer = new ConsumerBuilder<Ignore, string>(config)
            using (var consumer = new ConsumerBuilder<string, string>(config)
                // 如果不指定序列化类型，Confluent.Kafka 内部字典defaultDeserializers
                /*
                 *    private Dictionary<Type, object> defaultDeserializers = new Dictionary<Type, object>
               {
                { typeof(Null), Deserializers.Null },
                { typeof(Ignore), Deserializers.Ignore },
                { typeof(int), Deserializers.Int32 },
                { typeof(long), Deserializers.Int64 },
                { typeof(string), Deserializers.Utf8 },
                { typeof(float), Deserializers.Single },
                { typeof(double), Deserializers.Double },
                { typeof(byte[]), Deserializers.ByteArray }
                };
                 */
                // 会根据key 类型反射获取对应的序列化类型
                .SetValueDeserializer(Deserializers.Utf8)
                .SetKeyDeserializer(Deserializers.Utf8)

                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) =>
                    {
                        //Console.WriteLine($"Statistics: {json}")
                    }

                )
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                    // possibly manually specify start offsets or override the partition assignment provided by
                    // the consumer group by returning a list of topic/partition/offsets to assign to, e.g.:
                    // 
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .SetOffsetsCommittedHandler((iConsumer, committedOffsets) =>
                {
                   // iConsumer.
                   // committedOffsets.Offsets
                })
                .Build())
            {
                consumer.Subscribe(topics);

                try
                {
                    while (true)
                    {
                        try
                        {
                            // 返回 ConsumeResult<TKey, TValue> 包含：toppic、分区、当前消费的偏移量
                            var consumeResult = consumer.Consume(cancellationToken);

                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine($"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                                continue;
                            }
                           
                            //读取的key 是空
                            Console.WriteLine($"Received message at TopicPartitionOffset: {consumeResult.TopicPartitionOffset}:,Key: {consumeResult.Key},Value: {consumeResult.Value}");

                            //每消费5个提交一次偏移量，指向下一个待销费的偏移量
                            if (consumeResult.Offset % commitPeriod == 0)
                            {
                                // The Commit method sends a "commit offsets" request to the Kafka
                                // cluster and synchronously waits for the response. This is very
                                // slow compared to the rate at which the consumer is capable of
                                // consuming messages. A high performance application will typically
                                // commit offsets relatively infrequently and be designed handle
                                // duplicate messages in the event of failure.
                                try
                                {
                                    /*
                                     * kafka和很多消息系统不一样，很多消息系统是消费完了我就把它删掉，而kafka是根据时间策略删除，
                                     * 而不是消费完就删除（默认保存一周），在kafka里面没有一个消费完这么个概念，只有过期这样一个概念。
                                     */
                                    consumer.Commit(consumeResult);
                                }
                                catch (KafkaException e)
                                {
                                    Console.WriteLine($"Commit error: {e.Error.Reason}");
                                }
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
        }

        /// <summary>
        ///     In this example
        ///         - consumer group functionality (i.e. .Subscribe + offset commits) is not used.
        ///         - the consumer is manually assigned to a partition and always starts consumption
        ///           from a specific offset (0).
        /// </summary>
        public static void Run_ManualAssign(string brokerList, List<string> topics, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                // the group.id property must be specified when creating a consumer, even 
                // if you do not intend to use any consumer group functionality.
                GroupId = new Guid().ToString(),
                BootstrapServers = brokerList,
                // partition offsets can be committed to a group even by consumers not
                // subscribed to the group. in this example, auto commit is disabled
                // to prevent this from occurring.
                EnableAutoCommit = true
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build())
            {
                consumer.Assign(topics.Select(topic => new TopicPartitionOffset(topic, 0, Offset.Beginning)).ToList());

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cancellationToken);
                            // Note: End of partition notification has not been enabled, so
                            // it is guaranteed that the ConsumeResult instance corresponds
                            // to a Message, and not a PartitionEOF event.
                            Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Value}");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
        }

        private static void PrintUsage() => Console.WriteLine("Usage: .. <subscribe|manual> <broker,broker,..> <topic> [topic..]");

        public void Test()
        {
            var mode = "subscribe";
            var brokerList = "localhost: 9092";
            var topics = new List<string> { "topicName" };

            Console.WriteLine($"Started consumer, Ctrl-C to stop consuming");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            switch (mode)
            {
                case "subscribe":
                    Run_Consume(brokerList, topics, cts.Token);
                    break;
                case "manual":
                    Run_ManualAssign(brokerList, topics, cts.Token);
                    break;
                default:
                    PrintUsage();
                    break;
            }
        }

    }
}
