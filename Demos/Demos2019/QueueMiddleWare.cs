using Common;
using Demos.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class QueueMiddleWareDemo
    {
        // public static ConcurrentQueue<string> LogStr = new ConcurrentQueue<string>();
        public void Test()
        {
            //writeLog();

            readLog();
        }

        private void writeLog()
        {

            //QueueMiddleWare queueMiddleWare = new QueueMiddleWare((ipPort, command) =>
            //{
            //    //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  Producer:{ipPort} Commands:{command}");
            //    var str = $"Consumer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Producer:{ipPort} Commands:{command}";
            //    //  Log.Info<QueueMiddleWare>(str);
            //    //   QueueMiddleWareDemo.LogStr.Enqueue(str);
            //    BufferLog.LogAsync(str);
            //});
            QueueMiddleWare queueMiddleWare = new QueueMiddleWare();
            queueMiddleWare.ConsumerEventHandle += (ipPort, command) =>
            {
                //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  Producer:{ipPort} Commands:{command}");
                var str = $"Consumer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Producer:{ipPort} Commands:{command}";
                //  Log.Info<QueueMiddleWare>(str);
                //   QueueMiddleWareDemo.LogStr.Enqueue(str);
                BufferLog.LogAsync(str);
            };
            queueMiddleWare.Consumer();

            Task.Run(() =>
            {
                Dictionary<string, int> capacityDic = new Dictionary<string, int>();
                Random random = new Random();
                for(int j=1;j<=5;j++)
                {
                    for (int ithread = 1; ithread <= 20; ithread++)
                    {

                        Thread thread = new Thread((ithread) =>
                        {
                            //线程的start还未执行完，就执行下一次循环了。
                            string ipPort = $"port{ithread}";
                            if (!capacityDic.ContainsKey(ipPort))
                            {
                                var capacity = random.Next(10, 20);
                                capacityDic.Add(ipPort, capacity);
                            }
                            int count = random.Next(30, 50);
                            if(j>1)
                            {
                                count = random.Next(1, 10);
                            }
                            for (int icommand = 0; icommand < count; icommand++)
                            {
                                //Thread.Sleep(random.Next(1, 999));
                                //Thread.Sleep(1100);
                                queueMiddleWare.Producer(ipPort, $"{count}-{icommand + 1},Capacity{capacityDic[ipPort]}", capacityDic[ipPort]);
                            }
                        });
                        thread.Start(ithread);
                    }

                    Thread.Sleep(random.Next(1000, 2000));
                }
            });
        }
        private void readLog()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.txt");
            QueueMiddleWareLogAnalysis(filePath);
        }

        private void QueueMiddleWareLogAnalysis(string filePath)
        {
            List<string> content = TxtFile.ReadTxtFile(filePath);
            Dictionary<string, List<LogAnalysis>> operatorLogs = new Dictionary<string, List<LogAnalysis>>();
            content.ForEach(p =>
            {
                var logContentArray = p.Substring(24).Split(' ');
                LogAnalysis logAnalysis = new LogAnalysis();
                logAnalysis.Operation = logContentArray[0];
                logAnalysis.OperationTime = $"{logContentArray[1]} {logContentArray[2]}";
                logAnalysis.Operator = logContentArray[3];
                logAnalysis.Content = logContentArray[4];

                if (!operatorLogs.Keys.Contains(logAnalysis.Operator))
                {
                    operatorLogs.Add(logAnalysis.Operator, new List<LogAnalysis>() { logAnalysis });
                }
                else
                {
                    operatorLogs[logAnalysis.Operator].Add(logAnalysis);
                }
            });
            var count = operatorLogs.Values.Sum(p => p.Count) / 2;
            Console.WriteLine(count);
            foreach (var key in operatorLogs.Keys)
            {
                var list = operatorLogs[key].OrderBy(p => p.OperationTime).ToList();
                var operatorLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{DateTime.Now.Year}-{DateTime.Now.Month}/{key.Replace(':', '-')}.txt");
                TxtFile.SaveTxtFile(operatorLogFilePath, list.Select(p => p.ToString()).ToList());

            }

        }



    }
    public class LogAnalysis
    {
        public string Operation { get; set; }
        public string OperationTime { get; set; }
        public string Operator { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{Operation} {OperationTime} {Operator} {Content}";
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
        public event Action<string, string> ConsumerEventHandle;
        private ConcurrentDictionary<string, QueueData> _connectionData;
        private static Stopwatch _stopwatch = null;
        public QueueMiddleWare()
        {
            _connectionData = new ConcurrentDictionary<string, QueueData>();
            _stopwatch = Stopwatch.StartNew();
        }


        /// <summary>
        /// 所有生产者
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="command"></param>
        /// <param name="capacity"></param>
        public void Producer(string ipPort, string command, int capacity)
        {
            var str = $"Producer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Producer:{ipPort} Commands:{command}";
            //Log.Info<QueueMiddleWare>(str);
            //QueueMiddleWareDemo.LogStr.Enqueue(str);
            BufferLog.LogAsync(str);
            if (!this._connectionData.Keys.Contains(ipPort))
            {
                QueueData data = new QueueData(capacity);
                this._connectionData.TryAdd(ipPort, data);
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
        public void Consumer()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _stopwatch.Restart();
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
                    _stopwatch.Stop();
                    if (_stopwatch.ElapsedMilliseconds < 1)
                    {
                        //Log.Info<QueueMiddleWare>(_stopwatch.ElapsedMilliseconds.ToString());
                        //如果大量生产者数据造成拥塞，采用Stopwatch优化掉此1ms等待。
                        //不采用定时器，防止缓存队列过大拥塞产生的并发问题。
                        Thread.Sleep(1);
                    }
           
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
