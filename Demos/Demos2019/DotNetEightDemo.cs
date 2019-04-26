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
