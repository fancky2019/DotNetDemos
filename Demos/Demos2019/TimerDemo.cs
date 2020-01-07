using Demos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class TimerDemo
    {

        public void  Test()
        {
            Fun();
        }


        /// <summary>
        /// 设置成全局，避免GC回收造成只能执行一次。
        /// </summary>
        System.Threading.Timer _timer;

        private  void  Fun()
        {
            StopwatchHelper.Instance.Start();
            //2000:首次调用TimerCallback 的间隔（第一次调用TimerCallback 的间隔）
            //3000:第二次及之后每次调用TimerCallback 的间隔
            //两个参数一般设置一样。
            _timer = new System.Threading.Timer(p =>
            {
                StopwatchHelper.Instance.Stop();
                Console.WriteLine(StopwatchHelper.Instance.Stopwatch.ElapsedMilliseconds);
                StopwatchHelper.Instance.Start();
            }, null, 2000, 3000);
            return;
        }
    }
}
