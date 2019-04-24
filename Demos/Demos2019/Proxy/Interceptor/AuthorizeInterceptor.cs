using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy.Interceptor
{
    public class AuthorizeInterceptor : IInterceptor
    {
        AuthorizeAttribute _attribute;
        public AuthorizeInterceptor(AuthorizeAttribute attribute)
        {
            this._attribute = attribute;
        }
        public void OnActionExecuted()
        {
            Console.WriteLine("OnActionExecuted");

        }

        public void OnActionExecuting()
        {
            Console.WriteLine("OnActionExecuting");

        }
        public bool PreHandle()
        {
            return Authorization.Authorization == _attribute.Authorization;
        }
    }
}
