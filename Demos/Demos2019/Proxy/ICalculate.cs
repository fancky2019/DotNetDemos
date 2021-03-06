﻿using Demos.Demos2019.Proxy.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    public interface ICalculate
    {
        [Authorize]
        int Add(int a, int b);

        [Authorize(Authorization.UnAuthorization)]
        int Sub(int a, int b);
     }
}
