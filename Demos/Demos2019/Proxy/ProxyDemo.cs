using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    /// <summary>
    /// Aop：面向切面，代理，拦截器
    /// 官网实例：https://msdn.microsoft.com/zh-cn/library/dn574804.aspx
    /// </summary>
    class ProxyDemo
    {
        public void  Test()
        {
            //  IMessageSink
            //  ContextBoundObject
            // RealProxy
            //IProxy proxy = ProxyFactory.Create<Proxy>();

            //ICalculate proxy = ProxyFactory<Calculate>.Create();
            Calculate proxy = ProxyFactory<Calculate>.Create();
            proxy.Add(3,2);

            proxy.Sub(3, 2);
        }
    }
}
