using Demos.Demos2019.Proxy.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    class Calculate : MarshalByRefObject,ICalculate
    {
        [Authorize]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [Authorize(Authorization.UnAuthorization)]
        public int Sub(int a, int b)
        {
            return a - b;
        }
    }
}
