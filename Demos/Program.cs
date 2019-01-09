﻿using Demos.Demos;
using Demos.Demos.Reflection;
using Demos.Demos.SynchronizationDemo;
using Demos.Model;
using ManualResetEventDemo;
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
            //   new AutoResetEventTest().Test();
            // new ManualResetEventTest().Test();
            //  new ProducerConsumer(100).Test();

            //new ProducerConsumerTPS(100,5).Test();
            #endregion

            //  new BlockingCollectionDemo().Test();

            // new AttributeDemo().Test();
            new ImplicitExplicitDemo().Test();
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
