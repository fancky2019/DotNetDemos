using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos.SynchronizationDemo
{
    class SemaphoreTest
    {
        Semaphore _semaphore = new Semaphore(0, 3);
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
            _semaphore.Release(1);
            Console.ReadLine();
        }

        void Work()
        {
            try
            {
                _semaphore.WaitOne();
                Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                Thread.Sleep(3000);
                Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} completed!");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
