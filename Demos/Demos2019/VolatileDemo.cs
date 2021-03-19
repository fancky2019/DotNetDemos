using Demos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// volatile不支持原子操作，和Interlocked搭配使用
    /// 参见Common目录下InterLockedExtention类中使用
    /// 
    /// Volatile保证字段不缓存在CPU的寄存器中，而直接操作RAM（内存）
    /// 
    /// 如果你有多个线程对变量写入，volatile 无法解决你的问题，并且你必须使用 lock 来防止竞争条件。
    /// </summary>
    class VolatileDemo
    {
        public void Test()
        {
            Fun();
            //Fun1();
            //Fun2();
            //Fun3();
        }


        private volatile int _volParam = 0;

        private void Fun()
        {

            Thread thread1 = new Thread(() =>
             {
                 //volatile不支持原子操作
                 //_volParam 取值、++、赋值三个操作，不是原子操作。
                 //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                 for (int j = 0; j < 100000; j++)
                 {
                     ++_volParam;
                 }

             });
            Thread thread2 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread3 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread4 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread5 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }


            });
            Thread thread6 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread7 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread8 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread9 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            Thread thread10 = new Thread(() =>
            {
                //volatile不支持原子操作
                //_volParam 取值、++、赋值三个操作，不是原子操作。
                //因此此结果可能不是10。因为多个线程取值可能都是没++之后的值。
                for (int j = 0; j < 100000; j++)
                {
                    ++_volParam;
                }

            });
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
            thread6.Start();
            thread7.Start();
            thread8.Start();
            thread9.Start();
            thread10.Start();
            //for 循环内的线程可能未执行完_volParam可能不等于1000000；
            //等待所有线程执行完成
            while (thread1.ThreadState != ThreadState.Stopped ||
                   thread2.ThreadState != ThreadState.Stopped ||
                   thread3.ThreadState != ThreadState.Stopped ||
                   thread4.ThreadState != ThreadState.Stopped ||
                   thread5.ThreadState != ThreadState.Stopped ||
                   thread6.ThreadState != ThreadState.Stopped ||
                   thread7.ThreadState != ThreadState.Stopped ||
                   thread8.ThreadState != ThreadState.Stopped ||
                   thread9.ThreadState != ThreadState.Stopped ||
                   thread10.ThreadState != ThreadState.Stopped
                   )
            {

            }
            //存在并发问题，由于不支持原子操作所以可能_volParam!=100000
            Console.WriteLine(_volParam);
            Thread.Sleep(60000);

            //Thread.SpinWait
            Console.WriteLine(_volParam);
            Thread.Sleep(60000);
            Console.WriteLine(_volParam);
        }

        private void Fun1()
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    for (int j = 0; j <= 100000; j++)
                    {
                        _volParam = _volParam + 1;
                    }
                }).Start();
            }
            //for 循环内的线程可能未执行完_volParam值不定
            Console.WriteLine(_volParam);
            Thread.Sleep(60000);
            //此时_volParam可能已确定但是可能不等于1000000
            Console.WriteLine(_volParam);
            Thread.Sleep(60000);
            Console.WriteLine(_volParam);
        }

        private void Fun2()
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {

                    Interlocked.Increment(ref _volParam);
                    //while (true)
                    //{
                    //    if (InterLockedExtention.Acquire())
                    //    {
                    //        _volParam++;
                    //        InterLockedExtention.Release();
                    //        break;
                    //    }
                    //    SpinWait spinWait = default;
                    //    spinWait.SpinOnce();
                    //}

                }).Start();
            }
            Thread.Sleep(1000);

            //Thread t = null;
            //t.Join();

            Console.WriteLine(_volParam);
        }

        private void Fun3()
        {
            List<Action> actions = new List<Action>();
            for (int i = 0; i < 10; i++)
            {
                actions.Add(() =>
                {
                    Thread.Sleep(2000);
                    ++_volParam;
                });
            }
            //所有的任务完成前将阻塞该调用线程。
            Parallel.Invoke(actions.ToArray());
            Console.WriteLine(_volParam);//10
        }
    }
}
