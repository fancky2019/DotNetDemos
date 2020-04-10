﻿using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /*
     * 当lock(_lockObj):
     * 1):CLR分配内存：对象存储数据内存+类型对象指针+同步索引块
     * 2)：_lockObj的同步索引块会引用同步块数组（SyncBlock）中的同步块，同步块会记录当前线程的信息（如线程ID)。
     * 3):如果有其他线程试图进入lock块前，会判断同步块中线程的信息，是否是当前的线程。
     * 4):线程退出时候会将_lockObj的同步索引块设置-1。
     */
    class ThreadDemo
    {
        public void Test()
        {
            //ThreadParameter();

            ////this 参数
            //ThreadDemo1(this);

            //ThreadCreateUseTime();
            //LockMethod();
            //DeadLock();
            UserThreadDaemonThread();
        }
        private void ThreadDemo1(ThreadDemo threadDemo)
        {

        }

        private void TaskDemo1()
        {
            Task.Run(() =>
            {

            });
        }

        /// <summary>
        /// 线程创建的耗时不能确定0---100多ms,正常10几ms.
        /// </summary>
        private void ThreadCreateUseTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                stopwatch.Restart();
                new Thread(() =>
                 {
                     stopwatch.Stop();
                     Console.WriteLine(stopwatch.ElapsedMilliseconds);
                 }).Start();
            }
        }

        #region ThreadParameter
        private void ThreadParameter()
        {
            new Thread((parameter) =>
            {
                Console.WriteLine(parameter.ToString());
            }).Start("parameter");
            string objStr = "parameter";
            Task.Factory.StartNew((param) =>//带参数的回调
            {
                return "ThreadParameterr";
            }, objStr)//向Taskc传参ObjStr
            .ContinueWith(task =>
            {
                Object param = task.AsyncState;
                //如果前一个task 有返回值，则有result
                string result = task.Result;
            });


        }
        #endregion

        #region  线程的暂停、继续 AutoResetEvent、ManualReseDemos
        /*
         * AutoResetEvent、ManualReseDemos
         *参照Demo2018下的SynchronizationDemo文件夹下的 ProducerConsumer
         * 
         * 
         * 区别
         * ManualResetEvent在Set()之后,调用WaitOne()前必须调用Reset()，不然阻止不了
         * _produceManualResetEvent.Reset();
         * _produceManualResetEvent.WaitOne(); 
         * 
         * 
         * AutoResetEvent不用调用Reset
         */



        #endregion

        #region  锁
        private object _lockObj1 = new object();
        private object _lockObj2 = new object();
        int i = 0;
        private void LockMethod()
        {
            //Task.Run(() =>
            //{
            //    for(int i=0;i<100;i++)
            //    {
            //        RunMethod1();
            //    }
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        //RunMethod1();//i=200
            //        RunMethod2();
            //    }
            //});
            //i=200

            //for (int i = 0; i < 100; i++)
            //{
            //    Task.Run(() =>
            //    {
            //        //RunMethod1();//i=200
            //        RunLockMethod1();
            //    });
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    Task.Run(() =>
            //    {
            //        //RunMethod1();//i=200
            //        RunLockMethod2();
            //    });
            //}
            //i=200

            for (int i = 0; i < 100; i++)
            {
                Task.Run(() =>
                {
                    //runmethod1();//i=200
                    RunMethod();
                });
            }

            for (int i = 0; i < 100; i++)
            {
                Task.Run(() =>
                {
                    //runmethod1();//i=200
                    RunMethod1();
                });
            }
            Thread.Sleep(3000);
            Console.WriteLine(i);
        }

        private void RunLockMethod1()
        {
            lock (_lockObj1)
            {
                Console.WriteLine($"RunMethod1:{Thread.CurrentThread.ManagedThreadId}");
                i = i + 1;
            }
        }

        private void RunMethod()
        {
            //  Console.WriteLine($"RunMethod1:{Thread.CurrentThread.ManagedThreadId}");
            i = i + 2;
            Thread.Sleep(3000);
            Console.WriteLine($"i={i}");
        }

        private void RunMethod1()
        {
            Console.WriteLine($"RunMethod1:{Thread.CurrentThread.ManagedThreadId}");
            i = i + 2;
        }

        private void RunLockMethod2()
        {
            lock (_lockObj2)
            {
                Console.WriteLine($"RunMethod2:{Thread.CurrentThread.ManagedThreadId}");
                i = i + 1;
            }
        }

        #endregion

        #region 死锁

         Object _a = new Object();
         Object _b = new Object();

        /*
        当发生的死锁后，JDK自带了两个工具(jstack和JConsole)，可以用来监测分析死锁的发生原因。
        JConsole目录位置:C:\Java\jdk1.8.0_151\bin
                         使用方法：1）、选中一个进程，连接
                                   2）、在线程tab页下方，点击检测到死锁。
         */
        private void DeadLock()
        {
            try
            {
                /*
                线程产生死锁
                输出：
                 A-3 Enter Thread A
                 B-5 Enter Thread B
                */
                Task.Run(() =>
                 {
                     lock (_a)
                     {
                         try
                         {
                             Console.WriteLine($"A-{Thread.CurrentThread.ManagedThreadId} Enter Thread A");
                             //睡100ms,确保下面线程执行，否则下面线程还没执行，此线程就执行完，无法锁住。
                             Thread.Sleep(100);
                             int m = 0;
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine(ex.Message);
                         }
                         //线程会阻塞在此处。
                         lock (_b)
                         {
                             Console.WriteLine("A Enter Thread B");
                         }
                     }
                     Console.WriteLine("A  Thread  Complete");
                 });

                Task.Run(() =>
                 {
                     lock (_b)
                     {
                         try
                         {
                             Console.WriteLine($"B-{Thread.CurrentThread.ManagedThreadId} Enter Thread B");
                             //   Thread.sleep(100);
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine(ex.Message);
                         }
                         lock (_a)
                         {
                             Console.WriteLine("B Enter Thread A");
                         }
                     }
                     Console.WriteLine("B  Thread  Complete");
                 });

                //            CompletableFuture.runAsync(()->
                //            {
                //                while (true)
                //                {
                //
                //                }
                //            });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        #endregion

        #region 用户线程、守护线程
        /*
         * 
         *当程序有用户程序存在时候，守护线程就不会退出。
         *控制台在dos窗关闭时候前台线程可以退出
         * Winform窗体关闭，前台线程不能退出，后台线程可以退出。
         */
        volatile int _i = 1;

        private void UserThreadDaemonThread()
        {
            Thread userThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {

                        TxtFile.SaveTxtFile("UserThread.txt", new List<string>() { "userThread - " + _i.ToString() });

                        Thread.Sleep(1000);
                        ++_i;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            userThread.Start();

            Thread daemonThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        TxtFile.SaveTxtFile("DaemonThread.txt", new List<string>() { "daemonThread - " + _i.ToString() });
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            daemonThread.IsBackground = true;
            daemonThread.Start();
        }

        #endregion

        #region  while(true)线程退出
        bool _stop = false;
        //MSDN 官方实例
        // volatile  bool _stop=false;

        private void  ThreadExit()
        {
            /*
             * 存储结构：寄存器缓存+主存
             * volatile不在线程内缓存，会强制刷新到主存
             * 每个线程内会有一个变量的副本，线程改变变量会将改变量刷新到主存。
             * 
             * 
             * threadStop可能执行_stop为true,可能未及时刷新到主存，所以
             * thread可能还会继续运行。所以做好把_stop声明为volatile  bool _stop
             */
            Thread thread = new Thread(() =>
             {
                 while (!_stop)
                 {
                    //DoWork()
                }
             });
            thread.IsBackground = true;
            thread.Start();

            Thread threadStop = new Thread(() =>
             {
                 //当满足某个条件将_stop设为false。
                 _stop = true;
             });
            threadStop.IsBackground = true;
            threadStop.Start();
        }
        #endregion

        #region 线程状态
        private void ThreadState()
        {
            /*
             *ThreadState: Unstarted
             *ThreadState: WaitSleepJoin
             *ThreadState: Stopped
             */
            Thread newThread = new Thread(()=>
            {
                Thread.Sleep(1000);
            });

            Console.WriteLine("ThreadState: {0}", newThread.ThreadState);
            newThread.Start();

            // Wait for newThread to start and go to sleep.
            Thread.Sleep(300);
            Console.WriteLine("ThreadState: {0}", newThread.ThreadState);

            // Wait for newThread to restart.
            Thread.Sleep(1000);
            Console.WriteLine("ThreadState: {0}", newThread.ThreadState);

        }
        #endregion
    }
}
