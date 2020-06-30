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
            //Fun();
            DigitalFormat();
        }
        private void DigitalFormat()
        {

            var utcNow = DateTime.UtcNow;
            //整数默认int
            //小数默认double


            //数字后缀：f(F)、d(D)、m(M)

            byte byteParam = 3;
            short shortParam = 3;
            int intParam = 3;

            float floatParam = 0.03f;
            float floatParam1 = 0.03F;
            double doubleParam = 0.03d;
            double doubleParam1 = 0.03D;
            decimal decimalParam = 0.03m;
            decimal decimalParam1 = 0.03M;
            //错误
            //short s = shortParam + 3;
            //存在隐式转换。
            shortParam += 3;//===>shortParam=(short)(shortParam+3)

            var strVal = "";


            //标准数字格式字符串
            // 2.C - 货币表示，带有逗号分隔符，默认小数点后保留两位，四舍五入
            strVal = 2.5.ToString("C");//结果：￥ 2.50
                                       //     3.D - 十进制数
            strVal = 25.ToString("D5");//结果：00025
                                       //     4.F - 浮点数，保留小数位数(四舍五入)
            strVal = 25.ToString("F2");//结果：25.00
                                       //    5.G - 常规，保留指定位数的有效数字，四舍五入
            strVal = 2.52.ToString("G2");//结果：2.5 
                                         //  6.N - 带有逗号分隔符，默认小数点后保留两位，四舍五入
            strVal = 2500000.ToString("N");//结果：2,500,000.00
                                           //  7.X - 十六进制，非整型将产生格式异常
            strVal = 255.ToString("X");//结果：FF 

            int month = 2;
            var monthStr = string.Format("{0:00}", month);//02
            monthStr = month.ToString("D2");//02


            //保留小数位
            double doubleRe = Math.Round(1.2502, 3);//1.25
            doubleRe = Math.Round(1.2, 3);//1.2

            decimal d = 14.100m;
            var re = d.ToString("F2");//14.10
            d = 14.1m;
            re = d.ToString("F2");//14.10
            re = d.ToString("F5");//14.10000

            string.Format("{0:0.00%}", 1234);//结果： 123400.00%






           // 自定义数字格式字符串

            /*
             * 0:用数字去替换零，没有数字就显示零。
             * #：用数字去替换#，没有数字就不显示。
             * .:小数
             */

            //   1.0 零占位符  用对应的数字（如果存在）替换零；否则，将在结果字符串中显示零。
            strVal = string.Format("{0:000000}", 1234);//结果：001234

            //  2. “#”描述：数字占位符  用对应的数字（如果存在）替换“#”符号；
            //否则，不会在结果字符串中显示任何数字。

 

            strVal = string.Format("{0:######}", 1234);//结果：1234
            strVal = string.Format("{0:#0####}", 1234);//结果：01234
            strVal = string.Format("{0:0#0####}", 1234);//结果：0001234
           //  3. "."描述：小数点
            strVal = 123.45678.ToString($"0.###");//123.457
            strVal = 123.4.ToString($"0.###");//123.4
            strVal = 123.45678.ToString($"0.000");//123.457
            strVal = 123.4.ToString($"0.000");//123.400
            strVal = string.Format("{0:000.000}", 1234);//结果：1234.000
            strVal = string.Format("{0:000.000}", 4321.12543);//结果：4321.125
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

            //@号后的双引号转义 ""（两个双引号）
            string ss = @"""ddd";
            //错误
            string ss1 = @"\""ddd";
            //没有@的"\"转义
            string ss2 = "\"ddd";
            string sss = "{";

            //$ 字符串插值字符串中 花括号要转义
            /***
             * {-->{{
             * }-->}}
            */
            String script1 = $@"  {{
                  ""query"": {{
                   ""match"": {{
                     ""id"":{0}
                   }}
                 }},
                  ""script"": {{
                    ""lang"": ""painless"",
                   ""source"": ""ctx._source.name=params.name;ctx._source.age=params.age"",
                   ""params"": {{
                      ""name"": ""{"fancky"}"",
                     ""age"":{27}
                   }}
                 }}
                }}";

            //
            String script = string.Format("  {\n" +
          "          \"query\": {\n" +
          "            \"match\": {\n" +
          "              \"id\":{0}\n" +
          "            }\n" +
          "          },\n" +
          "          \"script\": {\n" +
          "            \"lang\": \"painless\",\n" +
          "            \"source\": \"ctx._source.name=params.name;ctx._source.age=params.age\",\n" +
          "            \"params\": {\n" +
          "              \"name\": \"{1}\",\n" +
          "              \"age\":{2}\n" +
          "            }\n" +
          "          }\n" +
          "        }", 1, "fancky", 27);




        }



        /// <summary>
        /// 四舍五入保留小数位后,去除小数位后多余的零
        /// </summary>
        /// <param name="decimalString">要去除多余零的数字字符串</param>
        /// <param name="decimalCount">最多保留的小数点位数</param>
        /// <returns></returns>
        public static string RemoveRedundantZero(string decimalString, int decimalCount)
        {
            float number = float.Parse(decimalString);
            //.##表示最多保留2位有效数字
            StringBuilder formatStr = new StringBuilder();
            formatStr.Append('#', decimalCount);
            // 四舍五入
            var newdecimalString = number.ToString($"0.{formatStr.ToString()}");
            return newdecimalString;
        }


        public static string SaveSpecifyCountDecimal(decimal dec, int count)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0.");
            sb.Append('0', count);
            var str = dec.ToString(sb.ToString());
            return str;
        }


    }
}
