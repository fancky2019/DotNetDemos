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
        DateTime dt = DateTime.ParseExact("20190528", "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}
