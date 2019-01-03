using Demos.Demos;
using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            //new ParamsDemo().Test();
            // string str = Test().Result;
            //   new AdoTest().Test();

            //new TClassTest<Product>().Test();
            //new LockDemo().Test();

            #region SynchronizationDemo
            #endregion

            new BlockingCollectionDemo().Test();

            int m = 0;
            Console.ReadLine();
        }
        static void Fundd()
        {
            Te();
        }
        static async void Te()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            string str = await Test();

        }
        static async Task<string> Test()
        {
            return await Task.Run(() =>
               {
                   Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                   Thread.Sleep(10 * 1000);
                   Console.WriteLine("dssdsd");
                   return "sdsdsd";
               });

        }
    }
}
