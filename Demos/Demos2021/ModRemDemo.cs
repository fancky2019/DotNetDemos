using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021
{
    /**
     *C#、java里： 取模=取余。获取余数
     */
    class ModRemDemo
    {
        public void Test()
        {
            Fun();
        }

        /*
         * 数学概念上的取余和取模
         * r=a%b
         * 符号一致时，求模运算和求余运算所得的c的值一致，因此结果一致；
         * 符号不一致时:求余运算结果的符号和a一致(截断法)，求模运算结果的符号和b一致(更小法)。
         */



        private void Fun()
        {
            int m = 15;
            int n = -20;
            var m1 = 5;
            var m2 = -4;
            var n1 = 3;
            var n2 = -7;


            var rm1 = m % m1;//0
            var rm2 = m % m2;// 15%-4=3 

            var rn1 = n % n1;// -20%3=-2
            var rn2 = n % n2;//-20%-7=-6 
            
        }
    }
}
