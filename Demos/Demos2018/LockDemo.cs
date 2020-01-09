using Demos.Common;
using Demos.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    /// <summary>
    /// Lock (Monitor) 排它锁
    /// 测试发现：Debug下Monitor的性能好
    ///           Release下InterLockedExtention拓展好。
    ///           
    /// 
    /// Lock 实现同步。等待其他线程执行完，该线程才能访问临界区资源，而不至于A线程未访问完，数据被B线程修改了
    ///                造成线程不安全。
    ///   
    /// 数据能被其他线程修改就是线程不安全的。
    /// 
    ///静态变量：线程非安全:静态变量即类变量，位于方法区，为所有对象共享，共享一份内存，
    ///          一旦静态变量被修改，其他对象均对修改可见，故线程非安全。
    ///实例变量：单例模式（只有一个对象实例存在）线程非安全，非单例线程安全。
    ///           实例变量为对象实例私有，在虚拟机的堆中分配，若在系统中只存在一个此对象的实例，在多线程环境下，
    ///            “犹如”静态变量那样，被某个线程修改后，其他线程对修改均可见，故线程非安全；
    ///            如果每个线程执行都是在不同的对象中，那对象与对象之间的实例变量的修改将互不影响，故线程安全。
    ///局部变量：线程安全。每个线程执行时将会把局部变量放在各自栈帧的工作内存中，线程间不共享，故不存在线程安全问题。
    ///
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    ///局部变量:在线程栈内，属于调用线程。线程安全。每个线程执行时将会把局部变量放在各自栈帧的工作内存中，
    ///         线程间不共享，故不存在线程安全问题。      
    ///全局变量：
    ///        1):静态变量：非线程安全，是属于类级别，被所有对象共享，共享一份内存，一旦值被修改，
    ///                     则其他对象均对修改可见，故线程非安全。
    ///        2):实例变量：线程安全。每个线程执行都是在不同的对象中，那对象与对象之间的实例变量的修改将互不影响，故线程安全。

    /// </summary>
    class LockDemo
    {
        /*
         * 互斥条件：一个资源每次只能被一个进程使用，即在一段时间内某 资源仅为一个进程所占有。此时若有其他进程请求该资源，则请求进程只能等待。
          请求与保持条件：进程已经保持了至少一个资源，但又提出了新的资源请求，而该资源 已被其他进程占有，此时请求进程被阻塞，但对自己已获得的资源保持不放。
          不可剥夺条件:进程所获得的资源在未使用完毕之前，不能被其他进程强行夺走，即只能 由获得该资源的进程自己来释放（只能是主动释放)。
          循环等待条件: 若干进程间形成首尾相接循环等待资源的关系
         */

        static object _lock = new object();
        public void Test()
        {
            ////int i = 0;
            ////lock(i)//报错，lock 语句要求引用类型
            ////{

            ////}
            //TestLock(20);
            //Person person = new Person() { Age = 20 };
            //TestLock(person);
            //Person tp = new Person() { Age = 10 };
            //Thread thread1 = new Thread(() =>
            //{
            //    TwoThreadsRobLock(tp);
            //});
            //thread1.Name = "thread1";
            //thread1.Start();
            //Thread thread2 = new Thread(() =>
            //{
            //    TwoThreadsRobLock(tp);
            //});
            //thread2.Name = "thread2";
            //thread2.Start();
            //Thread thread3 = new Thread(() =>
            //{
            //    TwoThreadsRobLock(tp);
            //});
            //thread3.Name = "thread3";
            //thread3.Start();
            //Console.WriteLine($"Two Threads rob lock:");

            //LockFun();
            LockUsingTime();
            InterLockUsingTime();
            //WaitUtilGetLock();
        }

        //CLR PDF702页
        //同一个线程对锁定的同步块有权限
        //同步块包含：内核对象、拥有线程的ID、递归计数、等待线程计数
        void TestLock(int i)
        {
            lock (_lock)
            {
                if (i > 10)
                {
                    //线程ID一样
                    Console.WriteLine($"ThreadID={Thread.CurrentThread.ManagedThreadId},i={i}");
                    i--;
                    TestLock(i);
                }
            }
            Console.WriteLine("ds");
        }

        void TestLock(Person i)
        {
            lock (i)
            {
                if (i.Age > 10)
                {
                    Console.WriteLine($"i.MyProperty={i.Age}");
                    i.Age--;
                    TestLock(i);
                }
                //Console.WriteLine($" i.MyProperty={i.MyProperty}");
            }
            Console.WriteLine($" i.MyProperty={i.Age}");
        }

        void TwoThreadsRobLock(Person p)
        {
            try
            {
                Console.WriteLine($"CurrentThread Name={Thread.CurrentThread.Name}");
                lock (_lock)//Lock内出现异常，外部有异常处理，lock会释放锁。
                {
                    //Monitor.Enter(_lock);   //Monitor必须 Exit释放锁，其他线程才能进入
                    Console.WriteLine($"{Thread.CurrentThread.Name} comes in");
                    Console.WriteLine($" i.MyProperty={p.Age}");
                    int m = int.Parse("ds");
                    p.Age--;
                    //Monitor.Exit(_lock);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //Monitor.Exit(_lock);

            }
        }

        static void LockFun()
        {
            Person p = null;
            //MSDN解释
            //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。 
            //如果其他线程尝试进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放。
            //直到该对象被释放
            lock (p)//运行时报错参数为空异常，未被实例化，就没有同步索引块
            {

            }
        }


        private void WaitUtilGetLock()
        {
            Task.Run(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Work();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            });
            Task.Run(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Work();
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            });
        }
        private void SaveGetLock()
        {
            if (Monitor.TryEnter(_lock, 200))//如果在200ms内获得锁
            {

            }
        }
        private void Work()
        {
            lock (_lockObj)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Enter");
                Thread.Sleep(3000);
            }
        }
        private object _lockObj = new object();
        public void LockUsingTime()
        {
            Console.WriteLine("Lock uses time");
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Restart();
            for (int i = 0; i < 20000; i++)
            {
                //stopwatch.Restart();
                lock (_lockObj)
                {

                }
                //stopwatch.Stop();
                ////Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //Console.WriteLine(stopwatch.ElapsedTicks);
                //Console.WriteLine(stopwatch.ElapsedTicks* GetNanosecPerTick());
            }
            stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(stopwatch.ElapsedTicks);

        }

        public void InterLockUsingTime()
        {
            Console.WriteLine("InterLock uses time");
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Restart();
            for (int i = 0; i < 20000; i++)
            {
                //stopwatch.Restart();
                if (InterLockedExtention.Acquire())
                {
                    InterLockedExtention.Release();
                }
                //stopwatch.Stop();
                ////Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //Console.WriteLine(stopwatch.ElapsedTicks);
                //Console.WriteLine(stopwatch.ElapsedTicks * GetNanosecPerTick());
            }
            stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine(stopwatch.ElapsedTicks);

        }

        /// <summary>
        /// 获取当前系统一个时钟周期多少纳秒
        /// </summary>
        /// <returns></returns>
        public long GetNanosecPerTick()
        {
            //1秒(s) =100厘秒(cs)= 1000 毫秒(ms) = 1,000,000 微秒(μs) = 1,000,000,000 纳秒(ns) = 1,000,000,000,000 皮秒(ps)
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            return nanosecPerTick;
        }

        private long _nanosecPerTick = 0L;
        public long NanosecPerTick
        {
            get
            {
                if (_nanosecPerTick == 0L)
                {
                    //1秒(s) =100厘秒(cs)= 1000 毫秒(ms) = 1,000,000 微秒(μs) = 1,000,000,000 纳秒(ns) = 1,000,000,000,000 皮秒(ps)
                    _nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
                }
                return _nanosecPerTick;
            }
        }
    }

}
