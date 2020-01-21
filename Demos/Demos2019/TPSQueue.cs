using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StockAdapterHKEX
{

    class TPSQueue<T>
    {
        public int Capacity { get; }
        public int TPS { get; }

        private Queue<DateTime> executeTimeList = new Queue<DateTime>();
        //  AutoResetEvent _produceAutoResetEvent = new AutoResetEvent(false);
        AutoResetEvent _consumerAutoResetEvent = new AutoResetEvent(false);
        ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        //private ZDLogger _logger = null;
        //public TPSQueue(int capacity = int.MaxValue, int tps = int.MaxValue)
        //{
        //    Capacity = capacity;
        //    this.TPS = tps;
        //}


        static DateTime _createLogTime;
        private static Timer _timer;
        static volatile bool _logChanged;
        private object _lockObj = new object();
        AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public TPSQueue(int tps = int.MaxValue)
        {
            this.TPS = tps;
            //_logger = new SynWriteLogger("TPSQueue.log");
            //_logger.setLogLevel(1);
            _timer = new Timer((o) =>
            {
                if (DateTime.Now.Day != _createLogTime.Day)
                {
                    lock (_lockObj)
                    {
                        //_createLogTime = DateTime.Now;
                        //_logChanged = true;
                        //_logger.Dispose();
                        //_logger = new SynWriteLogger("TPSQueue.log");
                        //_logChanged = false;
                        //_autoResetEvent.Set();
                    }
                }
            }, null, 1000, 1000);
        }
        private void LogAsync(string msg)
        {
            //Task.Run(() =>
            //{
            //    //NetInfo netInfo = t as NetInfo;
            //    //if (netInfo != null)
            //    //{
            //    //    _logger.log(1, $"Enqueue:{netInfo.MyToString()}");
            //    //}
            //    //else if(t is string)
            //    //{
            //    //    _logger.log(1, $"{t}");
            //    //}
            //    //else
            //    //{
            //    //_logger.log(1, $"Enqueue:{t.ToString()}");
            //    //}
            //    _logger.log(1, msg);
            //});

            if (_logChanged)
            {
                _autoResetEvent.WaitOne();
            }
            //_logger.log(1, msg);
        }
        public void Producer(T t)
        {
            //while (_queue.Count == Capacity)
            //{
            //    _produceAutoResetEvent.WaitOne();
            //}
            _queue.Enqueue(t);
     
            //NetInfo netInfo = t as NetInfo;
            //if (netInfo != null)
            //{
            //  //  _logger.log(1, $"Enqueue:{netInfo.MyToString()}");
            //    LogAsync($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Enqueue:{netInfo.MyToString()}");
            //}
            _consumerAutoResetEvent.Set();
        }

        public void Cunsumer(Action<T> callBack)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    while (_queue.IsEmpty)
                    {
                        _consumerAutoResetEvent.WaitOne();
                    }
                    //如果执行等于TPS
                    if (executeTimeList.Count >= this.TPS)
                    {
                        DateTime firstTime = executeTimeList.Dequeue();
                        TimeSpan ts = DateTime.Now - firstTime;
                        //执行间隔小于1s，等待
                        if (ts.TotalMilliseconds <= 1000)
                        {
                            //多睡1ms
                            int sleep = 1000 - (int)ts.TotalMilliseconds + 1;
                            LogAsync($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Sleep:{sleep} milliseconds");
                            Thread.Sleep(sleep);
                            //  _logger.log(1, $"Sleep:{sleep} milliseconds");
                         
                        }
                    }


                    T t;
                    _queue.TryDequeue(out t);
                    // LogAsync(t);
                    //NetInfo netInfo = t as NetInfo;
                    //if (netInfo != null)
                    //{
                    //    //  _logger.log(1, $"Enqueue:{netInfo.MyToString()}");
                    //    LogAsync($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Dequeue:{netInfo.MyToString()}");
                    //}
                    DateTime dequeueTime = DateTime.Now;
                    this.executeTimeList.Enqueue(dequeueTime);
                    //    _produceAutoResetEvent.Set();

                    //callBack?.BeginInvoke(t, null, null);
                    //若回调耗时很少采用同步调用，异步启用新线程耗时不稳定（启动线程还要占用时间）。
                    callBack?.Invoke(t);
                }
            });
        }

        /// <summary>
        /// 等待队列内容全部消费
        /// </summary>
        public void WaitForConsumerCompleted()
        {
            while (!_queue.IsEmpty)
            {
                Thread.Sleep(1);
            }
        }

    }
}
