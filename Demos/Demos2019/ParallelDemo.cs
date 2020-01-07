using Demos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class ParallelDemo
    {
        public void Test()
        {
            Fun1();
        }
        private void Fun1()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(i);
            }
            List<int> threadIDs = new List<int>();
            StopwatchHelper.Instance.Start();
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(list, i =>
              {
                  threadIDs.Add(Thread.CurrentThread.ManagedThreadId);
                  Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} item:{i}");
                  Thread.Sleep(500);
              });
        
            //1000  92768MS
            StopwatchHelper.Instance.Stop();
            var distinctIDS = threadIDs.Distinct().ToList();
            Console.WriteLine($"耗时:{ StopwatchHelper.Instance.Stopwatch.ElapsedMilliseconds} MS");
            Console.WriteLine($"耗时:{ StopwatchHelper.Instance.ElapsedNanosecond()} NS");
        }
    }
}
