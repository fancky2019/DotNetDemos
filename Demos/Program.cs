using Demos.Demos2018.RabbitMQ;
using Demos.Demos2018.SynchronizationDemo;
using Demos.Demos2019;
using Demos.Demos2019.Collections;
using Demos.Demos2019.Proxy;
using Demos.Demos2019.Subjects;
using Demos.OpenResource.HPSocket;
using Demos.OpenResource.Redis;
using Demos.OpenResource.Redis.StackExchangeRedis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            #region Demos2018
            //new ParamsDemo().Test();
            // string str = Test().Result;
            //   new AdoTest().Test();

            //new TClassTest<Product>().Test();
            //new LockDemo().Test();

            #region SynchronizationDemo
            //   new AutoResetEventTest().Test();
            // new ManualResetEventTest().Test();
            //  new ProducerConsumer(100).Test();

            //   new ProducerConsumerTPS(int.MaxValue, 10).Test();
            //   new ProducerConsumerTPSDemo().Test();
            #endregion

            //  new BlockingCollectionDemo().Test();

            // new AttributeDemo().Test();
            //   new ImplicitExplicitDemo().Test();

            // new RabbitMQTest().Test();
            // new TryCatchFinallyReturnDemo().Test();

            //  new CollectionDemo().Test();
            // new RedisDemo().Test();
            #endregion

            #region Demos2019
            //new EqualsOperatorDemo().Test();
            //  new ThreadDemo().Test();
            //  new StringInternDemo().Test();

            // new HPSocketDemo().Test();
            //   new CharDemo().Test();

            //new SubjectTest().Test();
            //new DivisionDemo().Test();
            //   new StackTraceDemo().Test();
            //  new ProxyDemo().Test();
            // new StackExchangeRedisDemo().Test();

            new NewOverrieDemo().Test();
            
            #endregion

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
