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
            LockMethod();
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


    }
}
