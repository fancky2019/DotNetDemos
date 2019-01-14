using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018.SynchronizationDemo
{
    class AutoResetEventTest
    {

        //若要将初始状态设置为终止，则为 true；若要将初始状态设置为非终止，则为 false。 
        //AutoResetEvent调用Set()方法之后会自动调用Reset()方法，调用WaintOne()会阻止。
        //ManualResetEvent 调用Set()方法之后必须调用Reset方法，不然调用WaintOne()不会阻止。
        AutoResetEvent _are = new AutoResetEvent(false);
        public void Test()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Work();
                }
            });


            Task.Run(() =>
            {
                while (true)
                {
                    Work();
                }
            });
            _are.Set();
            Console.ReadLine();
        }

        void Work()
        {
            try
            {
                _are.WaitOne();
                Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId} enter work()");
                Thread.Sleep(2000);
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
