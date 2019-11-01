using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class QueueMiddleWareDemo
    {
        public void Test()
        {
            QueueMiddleWare queueMiddleWare = new QueueMiddleWare((ipPort, command) =>
           {
               //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  Producer:{ipPort} Commands:{command}");
               var str = $"Consumer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  Producer:{ipPort} Commands:{command}";
               Log.Info<QueueMiddleWare>(str);


           });
            Task.Run(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        Random random = new Random();
                        int count = random.Next(50, 100);
                        var capacity = random.Next(20, 30);
                        for (int c = 0; c < count; c++)
                        {
                            //Thread.Sleep(random.Next(1, 999));
                            //Thread.Sleep(1100);
                            queueMiddleWare.Producer(Thread.CurrentThread.ManagedThreadId.ToString(), $"{Thread.CurrentThread.ManagedThreadId} Command{count}-{c + 1} Capacity{capacity}", capacity);
                        }
                    });
                    thread.Start();
                }
            });
        }
    }
    /// <summary>
    /// 每个连接只允许每秒只允许消费Capacity个任务
    /// </summary>
    public class QueueMiddleWare
    {
        /// <summary>
        /// param1:ipPort
        /// param2:command
        /// </summary>
        private event Action<string, string> ConsumerEventHandle;
        private Dictionary<string, QueueData> _connectionData;
        public QueueMiddleWare(Action<string, string> consumerEventHandle)
        {
            _connectionData = new Dictionary<string, QueueData>();
            this.ConsumerEventHandle = consumerEventHandle;
            Consumer();
        }


        /// <summary>
        /// 所有生产者
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="command"></param>
        /// <param name="capacity"></param>
        public void Producer(string ipPort, string command, int capacity)
        {
            var str = $"Producer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  Producer:{ipPort} Commands:{command}";
            Log.Info<QueueMiddleWare>(str);
            if (!this._connectionData.Keys.Contains(ipPort))
            {
                QueueData data = new QueueData(capacity);
                this._connectionData.Add(ipPort, data);
            }

            var queueData = _connectionData[ipPort];
            if (queueData.ConsumeredTimesCountEqualCapacity)
            {
                /*
                 * 如果缓存的命令为空，需要判断对头时间和Now间隔是否超过1s决定是否消费，
                 * 如果命令不为空，直接入队。
                 */
                if (queueData.Commands.IsEmpty)
                {
                    DateTime headerTime;
                    queueData.ConsumeredTimes.TryPeek(out headerTime);
                    var duration = DateTime.Now - headerTime;
                    /*
                     * 对量容量已满，如果队头时间和now间隔超过一秒直接消费，否则入队处理
                     */
                    if (duration.TotalMilliseconds > 1000)
                    {
                        /*
                         * 每次消费都要动态维护ConsumeredTimes队列。
                         * ConsumeredTimes队列中始终保存Capacity个最新消费的时间。                       
                         */
                        queueData.ConsumeredTimes.TryDequeue(out _);
                        queueData.ConsumeredTimes.Enqueue(DateTime.Now);

                        ConsumerEventHandle?.Invoke(ipPort, command);
                    }
                    else
                    {
                        queueData.Commands.Enqueue(command);
                    }
                }
                else
                {
                    queueData.Commands.Enqueue(command);
                }
            }
            else
            {
                //队列未满直接将消费时间入队，同时消费
                queueData.ConsumeredTimes.Enqueue(DateTime.Now);
                ConsumerEventHandle?.Invoke(ipPort, command);
            }
        }

        /// <summary>
        /// 一个消费者线程消费所有的生产者。
        /// 如果后期消费者消费不过来（造成队列拥塞），再优化消费者。
        /// </summary>
        private void Consumer()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    //循环遍历所有的消费
                    for (int i = 0; i < _connectionData.Keys.Count; i++)
                    {
                        var key = _connectionData.Keys.ElementAt(i);
                        var queueData = _connectionData[key];
                        /*
                         * 循环消费该连接的Commands，直到发送间隔小于1s或者消费完。
                         */
                        while (!queueData.Commands.IsEmpty)
                        {
                            DateTime headerTime;
                            queueData.ConsumeredTimes.TryPeek(out headerTime);
                            var duration = DateTime.Now - headerTime;
                            /*
                             * 如果队头时间和now间隔超过一秒直接消费，否则跳出当前连接的消费
                             */
                            if (duration.TotalMilliseconds > 1000)
                            {
                
                                /*
                                  * 每次消费都要动态维护ConsumeredTimes队列。
                                  * ConsumeredTimes队列中始终保存Capacity个最新消费的时间。                       
                                  */
                                queueData.ConsumeredTimes.TryDequeue(out _);
                                queueData.ConsumeredTimes.Enqueue(DateTime.Now);

                                string command;
                                queueData.Commands.TryDequeue(out command);
                                ConsumerEventHandle?.Invoke(key, command);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    //如果大量生产者数据造成拥塞，采用Stopwatch优化掉此1ms等待。
                    //不采用定时器，防止缓存队列过大拥塞产生的并发问题。
                    Thread.Sleep(1);
                }
            });
        }
    }

    public class QueueData
    {
        /// <summary>
        /// 消费的时间
        /// 只保存Capacity个最新的消费时间
        /// </summary>
        public ConcurrentQueue<DateTime> ConsumeredTimes { get; set; }
        /// <summary>
        /// 生产者的任务
        /// </summary>
        public ConcurrentQueue<string> Commands { get; set; }
        /// <summary>
        /// 每秒可消费的最大数量
        /// </summary>
        public readonly int Capacity;

        /// <summary>
        /// 避免每次获取Count都遍历链表的长度
        /// </summary>
        private bool _consumeredTimesCountEqualCapacity;

        /// <summary>
        ///消费的个数达到Capacity
        /// </summary>
        public bool ConsumeredTimesCountEqualCapacity
        {
            get
            {
                if (!_consumeredTimesCountEqualCapacity)
                {
                    _consumeredTimesCountEqualCapacity = (ConsumeredTimes.Count == Capacity);
                }
                return _consumeredTimesCountEqualCapacity;
            }
        }

        public QueueData(int capacity = 50)
        {
            ConsumeredTimes = new ConcurrentQueue<DateTime>();
            Commands = new ConcurrentQueue<string>();
            this.Capacity = capacity;
        }
    }
}
