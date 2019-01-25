using System;
using System.Collections.Generic;
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
            ThreadParameter();

            //this 参数
            ThreadDemo1(this);
        }
        private void ThreadDemo1(ThreadDemo threadDemo)
        {

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
    }
}
