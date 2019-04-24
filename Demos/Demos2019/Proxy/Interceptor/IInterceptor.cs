using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy.Interceptor
{
    public interface IInterceptor
    {
        void OnActionExecuted();
        void OnActionExecuting();

        bool PreHandle();
    }
}
