using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class ReflectionDemo
    {
        StackTrace _stackTrace = new StackTrace();
        public void Test()
        {
            Fun1();
            Fun2();
        }

        void  Fun()
        {
            //获取调用的方法
            Console.WriteLine(_stackTrace.GetFrame(1).GetMethod().Name);//不能用全局的，输出:Main
            Console.WriteLine(new StackTrace().GetFrame(1).GetMethod().Name);//输出Fun1,Fun2
        }
        void Fun1()
        {
            Fun();
        }
        void Fun2()
        {
            Fun();
        }
    }
}
