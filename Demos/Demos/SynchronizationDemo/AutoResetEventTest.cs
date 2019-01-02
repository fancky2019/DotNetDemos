using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos.SynchronizationDemo
{
    class AutoResetEventTest
    {

        AutoResetEvent _are = new AutoResetEvent(false);
        public void Test()
        {
            Task.Run(() =>
            {
                //Thread.Sleep(2000);
                //are.Set();


                Work();
            });


            Task.Run(() =>
            {
                Work();
            });




            //are.WaitOne();
            //Console.WriteLine("AutoResetEvent Completed");
            
            _are.Set();
            Console.ReadLine();
        }

        void Work()
        {
            try
            {
               // _are.Reset();
                _are.WaitOne();
                Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                Thread.Sleep(3000);
                Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} completed!");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _are.Set();
            }
        }
    }
}
