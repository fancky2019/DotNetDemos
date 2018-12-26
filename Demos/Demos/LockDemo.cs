using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos
{
    class LockDemo
    {
        static object _lock = new object();
        public void Test()
        {
            //int i = 0;
            //lock(i)//报错，lock 语句要求引用类型
            //{

            //}
            TestLock(20);
            Person person = new Person() { MyProperty = 20 };
            TestLock(person);
            Person tp = new Person() { MyProperty = 10 };
            Thread thread1 = new Thread(() =>
            {
                TwoThreadsRobLock(tp);
            });
            thread1.Name = "thread1";
            thread1.Start();
            Thread thread2 = new Thread(() =>
            {
                TwoThreadsRobLock(tp);
            });
            thread2.Name = "thread2";
            thread2.Start();
            Thread thread3 = new Thread(() =>
            {
                TwoThreadsRobLock(tp);
            });
            thread3.Name = "thread3";
            thread3.Start();
            Console.WriteLine($"Two Threads rob lock:");

            LockFun();
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
                if (i.MyProperty > 10)
                {
                    Console.WriteLine($"i.MyProperty={i.MyProperty}");
                    i.MyProperty--;
                    TestLock(i);
                }
                //Console.WriteLine($" i.MyProperty={i.MyProperty}");
            }
            Console.WriteLine($" i.MyProperty={i.MyProperty}");
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
                    Console.WriteLine($" i.MyProperty={p.MyProperty}");
                    int m = int.Parse("ds");
                    p.MyProperty--;
                    //Monitor.Exit(_lock);
                }
            }
            catch(Exception ex)
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
    }
    class Person
    {
        public int MyProperty { get; set; }
    }
}
