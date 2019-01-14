using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class ImplicitExplicitDemo
    {
        public void Test()
        {
            int num = 2;
            StringInt stringInt = num;
            int num1 = (int)stringInt;
        }

    }

    class StringInt
    {
        private int _field;
        public StringInt()
        {

        }
        public StringInt(int field)
        {
            this._field = field;
        }

        public static implicit operator StringInt(int num)
        {
            return new StringInt(num);
        }

        public static explicit operator int(StringInt stringInt)
        {
            return stringInt._field;
        }
    }
}
