using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    public class ProxyFactory
    {
        public static IProxy Create()
        {
            var proxy = new Proxy();
            var dynamicProxy = new DynamicProxy<IProxy>(proxy);
            return dynamicProxy.GetTransparentProxy() as IProxy;
        }
    }
}
