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
            Minus();
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
    
    }
}
