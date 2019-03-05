using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.SynchronizationDemo
{
    class ProducerConsumerTPSDemo
    {
        public void Test()
        {
            ConcurrentQueueTPS<DateTime> concurrentQueueTPS = new ConcurrentQueueTPS<DateTime>(100, 8);
            //生产者线程
            Task.Run(() =>
            {
                for (int i = 1; i <= 1000; i++)
                {
                    concurrentQueueTPS.Producer(DateTime.Now);
                    int next = new Random().Next(80, 300);
                    Thread.Sleep(next);
                }
            });

            //消费者线程
            Task.Run(() =>
            {
                while (true)
                {
                    concurrentQueueTPS.Cunsumer(p =>
                    {
                        Console.WriteLine($"DoWork time:{p.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                        Thread.Sleep(3 * 1000);
                    });

                    Thread.Sleep(100);
                }
            });

            Console.ReadLine();
        }
    }

    class ConcurrentQueueTPS<T>
    {
        public readonly int MaxLength;
        /// <summary>
        /// 
        /// </summary>
        public int TPS { get; set; }

        private Queue<DateTime> executeTimeList = new Queue<DateTime>();
        AutoResetEvent _produceAutoResetEvent = new AutoResetEvent(false);
        AutoResetEvent _consumerAutoResetEvent = new AutoResetEvent(false);


        //ManualResetEvent _produceManualResetEvent = new ManualResetEvent(false);
        //ManualResetEvent _consumerManualResetEvent = new ManualResetEvent(false);

        ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        // object _lockObj = new object();

        //public ConcurrentQueueTPS() : this(int.MaxValue, 0)
        //{

        //}
        ////this串联构造函数
        //public ConcurrentQueueTPS(int maxLength) : this(maxLength, 0)
        //{

        //}

        public ConcurrentQueueTPS(int maxLength = int.MaxValue,int tps=int.MaxValue)
        {
            //if(tps<=0)
            //{
            //    throw new Exception("必须设置正确TPS的值(TPS的值为正整数)。");
            //}
            MaxLength = maxLength;
            this.TPS = tps;
        }


        //public void Test()
        //{
        //    //生产者线程
        //    Task.Run(() =>
        //    {
        //        for (int i = 1; i <= 1000; i++)
        //        {
        //            Producer(i);
        //        }
        //    });

        //    //消费者线程
        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            Cunsumer();
        //            Thread.Sleep(100);
        //        }
        //    });

        //    Console.ReadLine();
        //}

        public void Producer(T num)
        {
            ////(一)
            ////此处用if Cunsumer()内只能在_queue.Count == MaxLength - 1才能Set,不能每次都Set,
            ////不然当_queue.Count == MaxLength时，由于 Cunsumer()每次都Set,WaitOne()跟就阻止不住。
            //if (_queue.Count == MaxLength)
            //{
            //    _produceAutoResetEvent.WaitOne();
            //}

            //_queue.Enqueue(num);
            //Console.WriteLine($"Enqueue : {num}");
            //_consumerAutoResetEvent.Set();

            //(二)用while,Cunsumer()可以每次都Set，
            //while还会在循环一次，由于Cunsumer()内Set了，while再次循环的时候发现_queue.Count != MaxLength。会执行下去继续生产。
            //由于消费者耗时较大，生产者添加到MaxLength时候，发现Cunsumer()内还没有Set，调用WaitOne()就阻止了
            while (_queue.Count == MaxLength)
            {
                _produceAutoResetEvent.WaitOne();
            }

            _queue.Enqueue(num);
            //if (num is DateTime dt)
            //{
            //  //  DateTime dt = DateTime.Parse(num.ToString());
            //    //  
            //    Console.WriteLine($"Enqueue : {dt.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //}
            Console.WriteLine($"Enqueue : {num}");
            _consumerAutoResetEvent.Set();




            //while (_queue.Count == MaxLength)
            //{
            //    //ManualResetEvent在Set()之后,调用WaitOne()前必须调用Reset()，不然阻止不了
            //    _produceManualResetEvent.Reset();
            //    _produceManualResetEvent.WaitOne();
            //}
            //_queue.Enqueue(num);
            //Console.WriteLine($"Enqueue : {num}");
            //_consumerManualResetEvent.Set();

        }

        public void Cunsumer(Action<T> callBack)
        {

            //(一)
            //while (_queue.IsEmpty)
            //{
            //    _consumerAutoResetEvent.WaitOne();
            //}

            //int result = 0;
            //_queue.TryDequeue(out result);

            //Console.WriteLine($"TryDequeue : {result}");
            //if (_queue.Count == MaxLength - 1)
            //{
            //    _produceAutoResetEvent.Set();
            //}



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
                if (ts.Seconds < 1)
                {
                    //多睡1ms
                    Thread.Sleep(1000 - ts.Milliseconds + 1);
                }
            }


            T result;
            _queue.TryDequeue(out result);
            DateTime dequeueTime = DateTime.Now;
            this.executeTimeList.Enqueue(dequeueTime);
            _produceAutoResetEvent.Set();
            Console.WriteLine($"TryDequeue : {result} time:{dequeueTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}");



            //DoWork 

            // callBack?.Invoke(result);
            //异步执行，不影响生产者消费者队列
            callBack?.BeginInvoke(result,null,null);
            //while (_queue.IsEmpty)
            //{
            //    _consumerManualResetEvent.WaitOne();
            //}

            //int result = 0;
            //_queue.TryDequeue(out result);

            //Console.WriteLine($"TryDequeue : {result}");
            //_produceManualResetEvent.Set();




        }

    }
}
