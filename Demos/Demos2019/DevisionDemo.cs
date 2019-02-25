using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 浮点数不能用二进制精确表示
    /// </summary>
    class DivisionDemo
    {
        public void Test()
        {
            Division();
            Multiplicative();
            Additive();
            Subtraction();
            Decimal();
        }
        void Division()
        {
            int r = 3 / 2;//1
            int mod = 19 % 4;//3
            double d1 = 3d / 2d;//1.5

            double re = 0.3 / 0.1;//2.9999999999999996

            float f = 0.3f / 0.2f;//3.0
            if ((0.3 / 0.1) == 3)//false
            {
                int m = 0;
            }
            else
            {
                int m = 0;
            }
        }
        void Multiplicative()
        {
            double multi = 0.3 * 0.2;//0.06
            double multi1 = 0.3d * 0.2d;//0.06
            float fl1 = 0.3f * 0.2f;//0.0600000024
            float fl2 = 0.3F * 0.2F;//0.0600000024


            double d1 = 0.3;
            double d2 = 0.2;
            double rd = d1 * d2;//0.06
            double d11 = 0.3d;
            double d22 = 0.2d;
            double rd1 = d11 * d22;//0.06

            float f1 = 0.3f;
            float f2 = 0.2f;
            float rf = f1 * f2;//0.0600000024
            float m = 0;
        }

        void Additive()
        {
            float f1 = 0.3f + 0.2f;//0.5
            double d1 = 0.3 + 0.2;//0.5
            double dd2 = 0.05 + 0.01;//0.060000000000000005
            double d2 = 0.05 + 0.01;//0.060000000000000005
            int m = 0;
        }

        void Subtraction()
        {
            float f1 = 0.3f - 0.2f;//0.10000001
            double d1 = 0.3 - 0.2;//0.09999999999999998
            double dd2 = 0.05 - 0.01;//0.04
            double d2 = 0.05 - 0.01;//0.04
            int m = 0;
        }

        void Decimal()
        {
            //double d = 0.03;
            //decimal d1 = decimal.Parse(d.ToString());

            decimal d1 = 0.03m;
            decimal d2 = 0.02m;
            decimal r1 = d1 / d2;//1.5
            decimal r2 = d1 * d2;//0.0006
            decimal r3 = d1 + d2;//0.05
            decimal r4 = d1 - d2;//0.01
            int m = 0;
        }
    }
}
