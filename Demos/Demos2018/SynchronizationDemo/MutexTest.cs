using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Demos.Demos2018.SynchronizationDemo
{ 
    class MutexTest
    {
        //必须设置true，指示调用线程是否应具有互斥体的初始所有权
        Mutex _mutex = new Mutex(true);
        public void Test()
        {
            Task.Run(() =>
            {
                //mutex.WaitOne();
                //Console.WriteLine("Mutex Completed");
                Work();
            });

            Task.Run(() =>
            {

                Work();
            });
            //Thread.Sleep(2000);
            //mutex.ReleaseMutex();
            ////Thread.Sleep(5000);

            //首次必须释放让一个线程能够获取资源
            //不然Work()得不到资源，会死锁。
            _mutex.ReleaseMutex();
            Console.ReadLine();


        }

        void Work()
        {
            try
            {
                _mutex.WaitOne();
                Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                Thread.Sleep(3000);
                Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} completed!");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}
