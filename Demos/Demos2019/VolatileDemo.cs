using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class VolatileDemo
    {
        public void  Test()
        {
            //Fun();
            Fun1();
        }


        private volatile int _volParam = 0;

        private void Fun()
        {
            for(int i=0;i<10;i++)
            {
                new Thread(() =>
                {
                    ++_volParam;
                }).Start();
            }
            //for 循环内的线程可能未执行完_volParam可能不等于10；
            Console.WriteLine(_volParam);
        }

        private void Fun1()
        {
            List<Action> actions = new List<Action>();
            for (int i = 0; i < 10; i++)
            {
                actions.Add(() =>
                {
                    Thread.Sleep(2000);
                    ++_volParam;
                });
            }
            //所有的任务完成前将阻塞该调用线程。
            Parallel.Invoke(actions.ToArray());
            Console.WriteLine(_volParam);
        }
    }
}
