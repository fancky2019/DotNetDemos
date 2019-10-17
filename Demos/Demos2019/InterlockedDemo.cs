using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class InterlockedDemo
    {
        public void Test()
        {
            //Main();
            int a = 2;
            int b = 3;
            int c = 2;

            /*
             * 参数
                location1 
                类型： System.Int32
                一个变量，包含要添加的第一个值。 两个值的和存储在 location1 中。 

                value 
                类型： System.Int32
                要添加到整数中的 location1 位置的值。 

                返回值
                类型： System.Int32
                存储在 location1 处的新值。 

                异常
             */
            //newa=5
            int newa = Interlocked.Add(ref a, b);//原子操作a=a+b; 

            a = 2;
            b = 3;
            c = 2;
            /*
             * 参数
            location1 
            类型： System.Int32
            其值将与 comparand 进行比较并且可能被替换的目标。 

            value 
            类型： System.Int32
            比较结果相等时替换目标值的值。 

            comparand 
            类型： System.Int32
            与位于 location1 处的值进行比较的值。 

            返回值
            类型： System.Int32
            location1 中的原始值。 

             */

            //aa=2;
            var aa = Interlocked.CompareExchange(ref a, c, b);//原子操作，条件赋值

            a = 2;
            b = 3;
            c = 2;
            /*
             * 参数
            location1 
            类型： System.Int32
            要设置为指定值的变量。 

            value 
            类型： System.Int32
            location1 参数被设置为的值。 

            返回值
            类型： System.Int32
            location1 的原始值。 
             */

            //bb=2;
            var bb = Interlocked.Exchange(ref a, b);//将b赋值给a，返回之前的a的值。

            a = 2;
            b = 3;
            c = 2;
            /*
            * 参数
            location 
            类型： System.Int32
            其值要递增的变量。 

            返回值
            类型： System.Int32
            递增的值。
           */

            //rr=3;
            var rr = Interlocked.Increment(ref a);

            a = 2;
            b = 3;
            c = 2;
            /*
           * 参数
            location 
            类型： System.Int32
            其值要递减的变量。 

            返回值
            类型： System.Int32
            递减的值。 
           */
            //dd=1;
            var dd = Interlocked.Decrement(ref a);
        }
        private static int usingResource = 0;

        private const int numThreadIterations = 5;
        private const int numThreads = 10;

        void Main()
        {
            Thread myThread;
            Random rnd = new Random();

            for (int i = 0; i < numThreads; i++)
            {
                myThread = new Thread(new ThreadStart(MyThreadProc));
                myThread.Name = String.Format("Thread{0}", i + 1);

                //Wait a random amount of time before starting next thread.
                Thread.Sleep(rnd.Next(0, 1000));
                myThread.Start();
            }
        }

        private void MyThreadProc()
        {
            for (int i = 0; i < numThreadIterations; i++)
            {
                UseResource();

                //Wait 1 second before next attempt.
                Thread.Sleep(1000);
            }
        }

        //A simple method that denies reentrancy.
        bool UseResource()
        {
            //如果上一个线程未调用Exchange，此时usingResource==1，调用
            //Exchange方法返回1，！=0，不能进入代码块，实现锁的功能。
            //0 indicates that the method is not in use.
            if (0 == Interlocked.Exchange(ref usingResource, 1))
            {
                Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);

                //Code to access a resource that is not thread safe would go here.

                //Simulate some work
                Thread.Sleep(500);

                Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name);

                //将usingResource赋值0
                //Release the lock
                Interlocked.Exchange(ref usingResource, 0);
                return true;
            }
            else
            {
                Console.WriteLine("   {0} was denied the lock", Thread.CurrentThread.Name);
                return false;
            }
        }
    }
}
