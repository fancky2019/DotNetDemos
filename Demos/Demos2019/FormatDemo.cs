using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-timespan-format-strings
    /// </summary>
    class FormatDemo
    {
        public void Test()
        {
            Fun();
        }

        private void Fun()
        {

            var yearMonth = "1908";
            //19
            var yearNumber = yearMonth.Substring(0, 2);
            //08
            var monthNumber = yearMonth.Substring(2, 2);
            var yearBegin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 2);

            var dateStr = $"{yearBegin}{yearNumber}{monthNumber}";
            DateTime contractDate = DateTime.ParseExact(dateStr, "yyyyMM", CultureInfo.InvariantCulture);
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);



            int i = 8;
            var iStr = i.ToString("D2");//08

            var m1 = "8".PadLeft(2, '0');//08
            var m2 = "10".PadLeft(2, '0');//10
            string intStr = "52";
            var str1 = Convert.ToDouble(intStr).ToString("0.00");

            double d = 10.434;
            var str2 = d.ToString("#0.00 ");//点后面几个0就保留几位
            var str3 = String.Format("{0:N2} ", d);//2位 

            var d1 = 52.00;
            //去除小数点后无效0
            string trimZero = d1.ToString("0.##");// .##表示最多保留2位有效数字
            var d2 = 52.10;
            string trimZero1 = d2.ToString("0.##");// .##表示最多保留2位有效数字

            var dateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
            TimeSpan ts = new TimeSpan(5, 16, 27, 29, 370);
            string tsStr = ts.ToString(@"dd\.hh\:mm\:ss");
            string tsStr1 = ts.ToString("c");
        }
    }
}
