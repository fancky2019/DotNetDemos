using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.SynchronizationDemo
{
    class ProducerConsumerTPS
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

        ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();

        ConcurrentQueue<string> _content = new ConcurrentQueue<string>();
        object _lockObj = new object();

        //this串联构造函数
        public ProducerConsumerTPS(int maxLength) : this(maxLength, 0)
        {

        }

        public ProducerConsumerTPS(int maxLength, int tps)
        {
            MaxLength = maxLength;
            this.TPS = tps;
        }


        public void Test()
        {
            //生产者线程
            Task.Run(() =>
            {
                for (int i = 1; i <= 1000; i++)
                {
                    Producer(i);
                    int next = new Random().Next(80, 300);
                    Thread.Sleep(next);
                }
            });

            //消费者线程
            Task.Run(() =>
            {
                while (true)
                {
                    Cunsumer();
                    //  Thread.Sleep(100);
                }


            });

            Console.ReadLine();
        }

        void Producer(int num)
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
            _consumerAutoResetEvent.Set();


            string msg = $"Enqueue : {num} time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
            // Console.WriteLine($"Enqueue : {num} time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");

            _content.Enqueue(msg);
            //  Console.WriteLine(msg);
            //  WriteLog(msg);





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

        void WriteLog(string content)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            using (StreamWriter sWriter = new StreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write), System.Text.Encoding.ASCII))
            {
                while (!_content.IsEmpty)
                {
                    if (_content.TryDequeue(out string msg))
                    {
                        sWriter.WriteLine(msg);
                    }
                }
            }
        }
        int _count = 0;
        void Cunsumer()
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
                if (ts.TotalMilliseconds <= 1000)
                {
                    //多睡1ms
                    Thread.Sleep(1000 - (int)ts.TotalMilliseconds + 1);
                }
            }

            int result = 0;
            _queue.TryDequeue(out result);
            DateTime dequeueTime = DateTime.Now;
            this.executeTimeList.Enqueue(dequeueTime);
            _produceAutoResetEvent.Set();



            string msg = $"TryDequeue : {result} time:{dequeueTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
            //  Console.WriteLine($"TryDequeue : {result} time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            _content.Enqueue(msg);
            //  Console.WriteLine(msg);
            //  WriteLog(msg);
            _count++;
            if (_count == 1000)
            {
                WriteLog("");

                Console.WriteLine("1000");
            }




            //DoWork 






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
