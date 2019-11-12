﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class ThreadDemo
    {
        public void Test()
        {
            //ThreadParameter();

            ////this 参数
            //ThreadDemo1(this);

            ThreadCreateUseTime();
        }
        private void ThreadDemo1(ThreadDemo threadDemo)
        {

        }

        private void TaskDemo1()
        {
            Task.Run(() =>
            {

            });
        }

        /// <summary>
        /// 线程创建的耗时不能确定0---100多ms,正常10几ms.
        /// </summary>
        private void ThreadCreateUseTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 30; i++)
            {
                stopwatch.Restart();
                new Thread(() =>
                 {
                     stopwatch.Stop();
                     Console.WriteLine(stopwatch.ElapsedMilliseconds);
                 }).Start();
            }
        }

        #region ThreadParameter
        private void ThreadParameter()
        {
            new Thread((parameter) =>
            {
                Console.WriteLine(parameter.ToString());
            }).Start("parameter");
            string objStr = "parameter";
            Task.Factory.StartNew((param) =>//带参数的回调
            {
                return "ThreadParameterr";
            }, objStr)//向Taskc传参ObjStr
            .ContinueWith(task =>
            {
                Object param = task.AsyncState;
                //如果前一个task 有返回值，则有result
                string result = task.Result;
            });


        }
        #endregion

        #region  线程的暂停、继续 AutoResetEvent、ManualReseDemos
        /*
         * AutoResetEvent、ManualReseDemos
         *参照Demo2018下的SynchronizationDemo文件夹下的 ProducerConsumer
         * 
         * 
         * 区别
         * ManualResetEvent在Set()之后,调用WaitOne()前必须调用Reset()，不然阻止不了
         * _produceManualResetEvent.Reset();
         * _produceManualResetEvent.WaitOne(); 
         * 
         * 
         * AutoResetEvent不用调用Reset
         */



        #endregion

    }
}
