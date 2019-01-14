using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManualReseDemos.Demos2018.SynchronizationDemotEventDemo
{
    class ManualResetEventTest
    {
        // mre is used to block and release threads manually. It is
        // created in the unsignaled state.
        private ManualResetEvent _mre = new ManualResetEvent(false);
        
        public void  Test()
        {
            Console.WriteLine("\nStart 3 named threads that block on a ManualResetEvent:\n");

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nWhen all three threads have started, press Enter to call Set()" +
                              "\nto release all the threads.\n");

            _mre.Set();//释放所有阻塞的线程
                      //When a ManualResetEvent is signaled, threads that call WaitOne() ndo not block..
                      //当调用Set()之后，线程调用WaitOne() 将不被锁住，除非再次调用Reset()


            Thread.Sleep(500);
            Console.WriteLine("\nWhen a ManualResetEvent is signaled, threads that call WaitOne()" +
                              "\ndo not block. Press Enter to show this.\n");
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Reset(), so that threads once again block" +
                              "\nwhen they call WaitOne().\n");


            _mre.Reset();//重新阻塞所有线程，ManualResetEvent 重置之后当调用WaitOne()之后又被锁住

            // Start a thread that waits on the ManualResetEvent.
            Thread t5 = new Thread(ThreadProc);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Set() and conclude the demo.");
     

            _mre.Set();

            // If you run this example in Visual Studio, uncomment the following line:
            //Console.ReadLine();
        }
        private  void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;

            Console.WriteLine(name + " starts and calls mre.WaitOne()");

            _mre.WaitOne();

            Console.WriteLine(name + " ends.");
        }
    }
}
