using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class RegexDemo
    {
        public void Test()
        {
            Match();
        }
        private void Match()
        {
            // Regex.Match(string input, string pattern);

            //DX_S1806 - 1809
           // string pattern= "[A-Z]+_S[0-9]{4}-[0-9]{4}";
            //DX_C1812 78
            // string pattern = @"[A-Z]+[_][CP][0-9]{4}[ ](-?(0|([1-9]\d*))\.?\d+)";
            //小数
            //-?(0|([1-9]\d*))\.\d+
            //数字
            string pattern = @"-?(0|([1-9]\d*))\.?\d+";
            //解析：
            //-?:0个或一个“-”（负号）。
            //(0|([1-9]\d*))：0或者([1-9]\d*)
            //([1-9]\d*):[1-9]中的一个数字，\d*：0个或多个数字。
            //\.?：可选的小数点
            //\d+：{1,}数字
          

            Regex regex = new Regex(pattern);
            var re = regex.IsMatch("78..2");


        }
    }


}
