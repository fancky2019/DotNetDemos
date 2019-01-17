using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class TryCatchFinallyReturnDemo
    {
        public void Test()
        {
            //try、catch有return 执行return之后再执行finally块，try、catch内return的值不受finally语句影响
            //因为try、catch在return之后才执行finally块
            Console.WriteLine($"Get()={Get()}");//异常：2，正常：1。
            Console.WriteLine($"a={a}");//2
        }
        int a = 1;
        int Get()
        {
            int m = -1;
            try
            {
                int i = int.Parse("a");
                return m = 1;
            }
            catch (Exception ex)
            {
                m = 0;
                a = 1;
                return m;
            }
            finally  //会在return后执行，之后离开方法体
            {
                a = 2;
                m = 2;
                //finally块内不能有return语句： 控制不能离开finally子句主题。java里可以有return并且覆盖try、catch里的return值
                //return m;
            }
        }
    }
}
