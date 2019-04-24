using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class StopwatchDemo
    {
        long _nanosecPerTick;
        public void Test()
        {
            _nanosecPerTick = GetNanosecPerTick();
            Fun();
            DisplayTimerProperties();
            CreateThreadWasteTime();
        }

        void Fun()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks* GetNanosecPerTick());


            //重置并重新计时
            stopwatch.Reset();
            stopwatch.Start();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks * GetNanosecPerTick());

            //Restart() = Reset() + Start()
            stopwatch.Restart();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks * GetNanosecPerTick());


            Stopwatch sp = Stopwatch.StartNew();
            sp.Stop();
            Console.WriteLine(sp.ElapsedTicks * GetNanosecPerTick());
        }



        public void DisplayTimerProperties()
        {
            // Display the timer frequency and resolution.
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                Console.WriteLine("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Console.WriteLine("  Timer frequency in ticks per second = {0}", frequency);
            //一个时钟周期多少纳秒
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine(" Timer tick is accurate within {0} nanoseconds", nanosecPerTick);
        }

        /// <summary>
        /// 获取当前系统一个时钟周期多少纳秒
        /// </summary>
        /// <returns></returns>
        public long GetNanosecPerTick()
        {
            //1秒(s) =100厘秒(cs)= 1000 毫秒(ms) = 1,000,000 微秒(μs) = 1,000,000,000 纳秒(ns) = 1,000,000,000,000 皮秒(ps)
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            return nanosecPerTick;
        }


        private  void  CreateThreadWasteTime()
        {
            Stopwatch stopwatch = new Stopwatch();
     

            for (int i= 0;i<= 100;i++)
            {
                stopwatch.Restart();
                //创建线程时间不等
                new Thread(() =>
                {
                    stopwatch.Stop();
                    //  Console.WriteLine($"CreateThreadWasteTime :{stopwatch.ElapsedTicks * GetNanosecPerTick()} ns");
                    Console.WriteLine($" ThreadID={Thread.CurrentThread.ManagedThreadId} CreateThreadWasteTime :{stopwatch.ElapsedTicks * _nanosecPerTick } ns");

                }).Start();
                //Task.Run(() =>
                //{
                //    stopwatch.Stop();
                ////  Console.WriteLine($"CreateThreadWasteTime :{stopwatch.ElapsedTicks * GetNanosecPerTick()} ns");
                //Console.WriteLine($" ThreadID={Thread.CurrentThread.ManagedThreadId} CreateThreadWasteTime :{stopwatch.ElapsedTicks * GetNanosecPerTick() / 1000000} ms");
                   
                //});
            }
        }


    }
}
