using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class DotNetEightDemo
    {
        private int n;

        public void Test()
        {
            Fun();
            //如果想要忽略变量，将该变量指定为弃元（_）。
            //弃元相当于未赋值的变量；它们没有值。
            //因为一个作用域名内只有一个弃元变量，甚至不为该变量分配存储空间，所以弃元可减少内存分配
            var (Name, _) = Info();
        }
        private void Fun()
        {
            //可空类型
            PointTest? pt = null;
            if (pt.HasValue)
            {
                var m = pt.Value.X;
            }
            else
            {
                pt = new PointTest();
                var m1 = pt.Value.X;
            }

            ////可空引用类型
            //PointClassTest? pct = null;
            //var n = pct.X;
            //pct = new PointClassTest();
            //var n1 = pct.X;
        }

        private (string Name, int Age) Info()
        {
            return ("Fancky", 27);
        }

        /// <summary>
        /// 独立弃元:可使用独立弃元来指示要忽略的任何变量
        /// </summary>
        /// <param name="_"></param>
        /// <param name="name"></param>
        private void Fun1(string _, string name)
        {
            Console.WriteLine(_);

            ////一个块内只能有一个弃元，此处语法报错
            //if (int.TryParse("1",out _))
            //{

            //}
        }

        private void Fun2()
        {
            //一个块内只能有一个弃元，此处语法报错
            if (int.TryParse("1", out _))
            {

            }
        }

        private void Fun3()
        {
            //委托不想用参数，直接用弃元
            Action<string> action = _ => Console.WriteLine("callBack");
        }
    }

    struct PointTest
    {
        public int X;
        public int Y;

    }
    class PointClassTest
    {
        public int X { get; set; }
    }
}
