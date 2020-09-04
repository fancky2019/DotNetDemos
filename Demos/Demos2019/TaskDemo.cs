using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class TaskDemo
    {
        /*
         * Task<TResult>：表示一个可以返回值的异步操作。Task<TResult> 生成的结果的类型。
         * Task：表示一个异步操作。
         */

        /**
          * async 表示异步方法。异步方法可以具有 Task、 Task<TResult>或无效的返回类型
          * async和await 成对使用。
          * Task<int>：有返回值Result.可await
          * Task:无返回值Result,但是可判断task的执行结果。可await
          * void .不能await
          * 
          * async 方法内部使用Task操作具体任务。
          * 
          * await 只能在异步方法中使用。
          */

        public void Test()
        {
            //Fun1();
            //Fun2();
            NewTask();
            Fun3();
        }
        private void TaskThreadCounts()
        {
            //ThreadPool.SetMaxThreads
        }


        private async void Fun1()
        {
            // await只能出现在异步方法中，等待结果返回
            var re1 = await FunTask(1);
            var re2 = await FunTask(2);
            //如果调用异步方法，不想让方法加async。就用 task.Wait()
            var task = FunTask(3);
            task.Wait(3);//调用wait等待任务完成。
            var re3 = task.Result;//如果任务没有完成，将阻塞到任务完成，相当于调用wait。
        }

        private void Fun2()
        {
            //如果调用异步方法，不想让方法加async。就用 task.Wait()
            var task3 = FunTask(3);
            task3.Wait();//调用wait等待任务完成。
            var re3 = task3.Result;//如果任务没有完成，将阻塞到任务完成，相当于调用wait。

            var task4 = FunTask(3);
            var re4 = task4.Result;//如果任务没有完成，将阻塞到任务完成，相当于调用wait。

        }

        private void Fun3()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var task1 = FunAsyncTask(1);
            var task2 = FunAsyncTask(2);
            var task3 = FunAsyncTask(3);

            //不用每一个都await等待结果。
            //先启动所有任务，然后等待一个3s.不然每个都await要等待9s.耗时。
            Thread.Sleep(3000);
            var ta1 = task1.Result;


            var t1 = FunAsyncTask1(1);
            FunAsyncTask2(2);

        }

        /// <summary>
        /// 可等待
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Task<int> FunTask(int i)
        {
            return Task.Run(() =>
                 {
                     Thread.Sleep(3000);
                     return i;
                 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int FunTaskResult(int i)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(3000);
                return i;
            }).Result;
        }


        /// <summary>
        ///  返回Task<T> 对象，有返回值Result
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private async Task<int> FunAsyncTask(int i)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 1;
            });

            //或者
            //Task<int> task=  Task.Run(() =>
            //{
            //    Thread.Sleep(3000);
            //    return 1;
            //});
            //return task.Result;
        }


        /// <summary>
        /// 返回task 对象，没有返回值Result
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private async Task FunAsyncTask1(int i)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
             {
                 Thread.Sleep(3000);
                 return 1;
             });
            //方法返回：async Task，不需要return
            //return task;
        }

        /// <summary>
        /// 不返回task 对象
        /// </summary>
        /// <param name="i"></param>
        private async void FunAsyncTask2(int i)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 1;
            });
            //方法返回：async Task，不需要return
            //return task;
        }


        private void NewTask()
        {
            //run 内部：task.ScheduleAndStart(needsProtection: false);
            //已经调用start.
            Task.Run(() =>
            {

            });
            Task task = new Task((str) =>
              {
                  Thread.Sleep(3000);
                  Console.WriteLine(str);
              }, "test");
            //启动task。
            task.Start();

            //Task.WaitAll(task);

            Task.WhenAll(task).ContinueWith((task) =>
            {

            }); ;

        }
    }
}
