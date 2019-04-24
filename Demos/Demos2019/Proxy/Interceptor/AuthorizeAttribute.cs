using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy.Interceptor
{
    // AttributeTargets.All
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute//, ProxyAttribute
    {
        public Authorization Authorization { get; set; }
        public String Description { get; set; }
        public AuthorizeAttribute(Authorization authorization = Authorization.Authorization, string description = "")
        {
            this.Authorization = authorization;
            this.Description = description;
        }
    }

    public enum Authorization
    {
        Authorization,
        UnAuthorization
    }
}
