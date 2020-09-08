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
            //TypeEqual();
            InvokeMethod();
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

        private void InvokeMethod()
        {
            var parameters = new object[] { 8m, "chengxuyuan" };
            ReflectionClass instance = new ReflectionClass();
            var type = instance.GetType();
            var method = type.GetMethod("GetSalary");
            if (method != null)
            {
                var re = method.Invoke(instance, parameters);
            }
        }
    }

    class ReflectionClass
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public decimal GetSalary(decimal workHours, string jobName)
        {
            return 1000;
        }
    }

    public class NettyRequest
    {
        public string RequestGUID { get; set; }
        public string CommandName { get; set; }
        public object[] Parameters { get; set; }
    }

    public class NettyResponse
    {
        public string StatusCode { get; set; }
        public object Result { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class StatusCode
    {
        /// <summary>
        /// 请求失败
        /// </summary>
        public const string Success = "200";
        public const string ClientError = "400";
        public const string ServerError = "500";
    }
}
