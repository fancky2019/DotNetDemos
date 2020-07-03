using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class SemaphoreDemo
    {
        public void Test()
        {
            //Main();
            Test1();
        }

        // A semaphore that simulates a limited resource pool.
        //
        private Semaphore _semaphore;

        // A padding interval to make the output more orderly.
        private int _padding;

        public void Main()
        {
            // Create a semaphore that can satisfy up to three
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            //
            _semaphore = new Semaphore(0, 4);

            // Create and start five numbered threads. 
            //
            for (int i = 1; i <= 20; i++)
            {
                //Thread t = new Thread(new ParameterizedThreadStart(Worker));

                //// Start the thread, passing the number.
                ////
                //t.Start(i);


                Task.Run(() =>
                {
                    Worker(i);
                });
            }

            // Wait for half a second, to allow all the
            // threads to start and to block on the semaphore.
            //
            Thread.Sleep(5000);

            // The main thread starts out holding the entire
            // semaphore count. Calling Release(3) brings the 
            // semaphore count back to its maximum value, and
            // allows the waiting threads to enter the semaphore,
            // up to three at a time.
            //
            Console.WriteLine("Main thread calls Release(3).");


            //Release ；Worker调用WaitOne()阻塞当前线程，Release继续执行。
            //初始3个释放信号量，有3个线程将获得信号量。（允许最大并发数）。
            //不能超过信号量构造中指定的最大数，否则抛异常
            _semaphore.Release(3);

            Console.WriteLine("Main thread exits.");
        }

        private void Worker(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore.
            Console.WriteLine("Thread {0} begins and waits for the semaphore.", num);
            //阻止调用线程，知道获得信号量才能继续执行。
            _semaphore.WaitOne();

            // A padding interval to make the output more orderly.
            int padding = Interlocked.Add(ref _padding, 100);

            Console.WriteLine("Thread {0} enters the semaphore.", num);

            // The thread's "work" consists of sleeping for 
            // about a second. Each thread "works" a little 
            // longer, just to make the output more orderly.
            //
            Thread.Sleep(1000 + padding);

            Console.WriteLine("Thread {0} releases the semaphore.", num);
            //释放一个信号量，下一个等待的线程将进入
            Console.WriteLine("Thread {0} previous semaphore count: {1}", num, _semaphore.Release());
        }


        private void Test1()
        {
            //initialCount:初始可用的信号量计数。零：其他线程调用waitOne必须等待。
            _semaphore = new Semaphore(0, 4);

            Task.Run(() =>
            {
                Produce();
            });
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Consume();
            });
        }

        private void Produce()
        {
            //释放一个信号量，
            //_semaphore.Release();
        }

        private void Consume()
        {
            //如果生产者不释放信号量，将一直阻塞。
            _semaphore.WaitOne();
            //_semaphore.WaitOne(5000);
        }

    }
}
