using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    public class StopwatchHelper
    {
        public static StopwatchHelper Instance;
        public Stopwatch Stopwatch { get; set; }
        static StopwatchHelper()
        {
            Instance = new StopwatchHelper();

        }

        StopwatchHelper()
        {
            Stopwatch = Stopwatch.StartNew();
        }


        public  void Start()
        {
            Stopwatch.Restart();
        }

        public  void Stop()
        {
            Stopwatch.Stop();
        }

        public  long ElapsedNanosecond()
        {
          return Stopwatch.ElapsedTicks* GetNanosecPerTick();
        }

        /// <summary>
        /// 获取当前系统一个时钟周期多少纳秒
        /// </summary>
        /// <returns></returns>
        long GetNanosecPerTick()
        {
            //1秒(s) =100厘秒(cs)= 1000 毫秒(ms) = 1,000,000 微秒(μs) = 1,000,000,000 纳秒(ns) = 1,000,000,000,000 皮秒(ps)
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            return nanosecPerTick;
        }
    }
}
