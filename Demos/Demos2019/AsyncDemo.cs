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
    /// 当一个方法可以拆解成两个以上耗时的任务，可以用异步并行执行，提高吞吐量，
    /// 封装接口可以封装成异步的，当调用者有两个以上的任务时候就可以发挥异步提高吞吐量的优势。
    /// 否则只有一个耗时任务await 无法提高吞吐量。和同步没有区别。
    /// </summary>
    public class AsyncDemo
    {
        public void Test()
        {
            //AsyncMethod();
            // 异步方法有返回Task<TResult> 。调用要用await且调用方法async修饰，否则同步执行。
            //   await  AsyncMethod1();

            //异步方法无返回值，没有限制。
            Test1();
        }

        private async void Test1()
        {
            //异步方法有返回Task < TResult > 。调用要用await且调用方法async修饰，否则同步执行。
            int length = await AccessTheWebAsync();
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
