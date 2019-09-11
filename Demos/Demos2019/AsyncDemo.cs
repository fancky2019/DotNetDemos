using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 异步方法：void 不阻塞调用线程。
    ///         返回Task<T>，如果想得到返回的Task<T> :1、采用async 方法，内部使用await 关键字。会阻塞调用线程不好。
    ///                                               2、采用回调ContinueWith。建议采用此方法不阻塞调用线程。
    /// 
    /// 
    /// </summary>
    public class AsyncDemo
    {
        public void Test()
        {
            //AsyncMethod();
            // 异步方法有返回Task<TResult> 。调用要用await且调用方法async修饰，否则同步执行。
            //await AsyncMethod1();
            AsyncMethod1();
            //异步方法无返回值，没有限制。
            //Test1();

            //以下方法都不阻塞，直接执行通过
            TaskContinue();
            TestAsync();
            //Test2(); 内部没有使用await 同步方式运行，将阻塞。
            Console.WriteLine($"Test Function Completed!");
        }

        /// <summary>
        /// 异步方法 void：不阻塞调用线程，但是要是有返回值，要么等待，要么采用回调
        /// </summary>
        private async void TestAsync()
        {
            int result = await TaskContinueAsync();
            Console.WriteLine($"TestAsync TaskResult is {result}");
        }

        private async void Test1()
        {
            //异步方法有返回Task < TResult > 。调用要用await且调用方法async修饰，否则同步执行。
            int length = await AccessTheWebAsync();
        }

        /// <summary>
        /// async 方法内部没有使用await，将同步方式执行，即就是个同步方法。
        /// </summary>
        private async void Test2()
        {
            //异步方法有返回Task < TResult > 。调用要用await且调用方法async修饰，否则同步执行。
            Thread.Sleep(5000);
        }
        /// <summary>
        /// 采用异步回调：没有返回值，不阻塞
        /// </summary>
        private void TaskContinue()
        {
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                return 1;
            }).ContinueWith(task =>
            {
                Console.WriteLine($"TaskContinue TaskResult is {task.Result}");
            });
        }

        /// <summary>
        /// 采用异步返回Task<T>
        /// </summary>
        /// <returns></returns>
        private Task<int> TaskContinueAsync()
        {
            return Task.Run(() =>
             {
                 Thread.Sleep(5000);
                 return 1;
             });
        }

        private async void AsyncMethod()
        {
            DateTime dt1 = DateTime.Now;
            Task t1 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return 1;
            });
            Task t2 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return 2;
            });
            //阻塞5秒
            await Task.WhenAll(t1, t2);
            int seconds = (DateTime.Now - dt1).Seconds;
        }

        private async Task<int> AsyncMethod1()
        {
            DateTime dt1 = DateTime.Now;
            Task<int> t1 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return 1;
            });
            Task<int> t2 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return 2;
            });
            await Task.WhenAll(t1, t2);
            int result = t1.Result + t2.Result;
            int seconds = (DateTime.Now - dt1).Seconds;
            return seconds;
        }

        async Task<int> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.
            //  - AccessTheWebAsync can't continue until getStringTask is complete.
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.
            //  - Control resumes here when getStringTask is complete. 
            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        /// <summary>
        /// 做独立的耗时工作
        /// </summary>
        void DoIndependentWork()
        {
            Thread.Sleep(3000);
        }
    }

}
