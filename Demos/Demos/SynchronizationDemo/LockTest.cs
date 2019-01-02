using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos.SynchronizationDemo
{
    class LockTest
    {
        private object _lockObj = new object();
        public void  Test()
        {
            Task.Run(() =>
            {
                Work();
            });

            Task.Run(() =>
            {
                Work();
            });
            Console.ReadLine();
        }
        void Work()
        {
            lock (_lockObj)
            {
                try
                {
                    Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                    Thread.Sleep(3000);
                    Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} completed!");
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
            }
        }
    }
}
