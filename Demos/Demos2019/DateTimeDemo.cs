using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class DateTimeDemo
    {
        public void  Test()
        {
            //Minus();
            DateTimeToString();
        }

        private void ConvertExact()
        {
            DateTime dt = DateTime.ParseExact("20190528", "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private void Minus()
        {
            DateTime start = DateTime.Parse("2018-01-02 12:35:50");
            DateTime end = DateTime.Now;
            TimeSpan timeSpan = end - start;
        }

        private void DateTimeToString()
        {
            //2020-1-6
            var toShortDateString = DateTime.Now.ToShortDateString();
            //2020年1月6日
            var toLongDateString = DateTime.Now.ToLongDateString();
            //2020-01
            var yearMonthStr = DateTime.Now.ToString("yyyy-MM");
            //2020-01-06
            var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            //2020-01-06 13:41:38.890
            var longDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    
    }
}
