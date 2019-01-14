using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.SynchronizationDemo
{
    class InterlockedTest
    {
        private static int _usingResource = 0;

        public void Test()
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
            try
            {
                if (Interlocked.Exchange(ref _usingResource, 1)==0)
                {
                    Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                    Thread.Sleep(3000);
                    Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} exiting lock!");
                    //Release the lock
                    Interlocked.Exchange(ref _usingResource, 0);
                }
                else
                {
                    Console.WriteLine("Thread{0} was denied the lock", Thread.CurrentThread.ManagedThreadId);
                }
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
