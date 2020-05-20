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
            //Fun1();
            //Fun2();
            TypeEqual();
        }

        void Fun()
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

        private void TypeEqual()
        {
            //幂等性
            Type objectGetType = this.GetType();
            Type objectGetType1 = this.GetType();
            //true
            bool objectGetTypeEqual = objectGetType == objectGetType1;
           //幂等性
            Type typeofType = typeof(ReferenceDemo);
            Type typeofType1 = typeof(ReferenceDemo);
            //true 
            bool typeofTypeEqual = typeofType1 == typeofType;

            //false
            bool equal = objectGetType == typeofType;


        }
    }
}
