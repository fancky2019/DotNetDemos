using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos.SynchronizationDemo
{
    class ProducerConsumer
    {
        public readonly int MaxLength;
        public ProducerConsumer(int maxLength)
        {
            MaxLength = maxLength;
        }


        AutoResetEvent _produceAutoResetEvent = new AutoResetEvent(false);
        AutoResetEvent _consumerAutoResetEvent = new AutoResetEvent(false);


        //ManualResetEvent _produceManualResetEvent = new ManualResetEvent(false);
        //ManualResetEvent _consumerManualResetEvent = new ManualResetEvent(false);

        ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();

        object _lockObj = new object();
        public void Test()
        {
            //生产者线程
            Task.Run(() =>
            {
                for (int i = 1; i <= 1000; i++)
                {
                    Producer(i);
                }
            });

            //消费者线程
            Task.Run(() =>
            {
                while (true)
                {
                    Cunsumer();
                    Thread.Sleep(800);
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

            int result = 0;
            _queue.TryDequeue(out result);
            Console.WriteLine($"TryDequeue : {result}");
            _produceAutoResetEvent.Set();




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
