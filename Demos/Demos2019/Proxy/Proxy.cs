using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    class Proxy : IProxy
    {

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
