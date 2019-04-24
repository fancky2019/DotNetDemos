using Demos.Demos2019.Proxy.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    class DynamicProxy<T> : RealProxy
    {
        private readonly T _decorated;
        public DynamicProxy(T decorated)
          : base(typeof(T))
        {
            _decorated = decorated;
        }
        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }
        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            //  methodInfo.GetCustomAttributes
            Log("In Dynamic Proxy - Before executing '{0}'", methodCall.MethodName);
            try
            {
                //var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                //Log("In Dynamic Proxy - After executing '{0}' ", methodCall.MethodName);
                //return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);


                IInterceptor interceptor = GetIInterceptor(methodInfo);
               if(interceptor.PreHandle())
                {
                    interceptor.OnActionExecuting();
                    var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                    Log("In Dynamic Proxy - After executing '{0}' ", methodCall.MethodName);
                    interceptor.OnActionExecuted();
                    return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                }
                else
                {
                    return new ReturnMessage(new Exception ("UnAuthorization"), methodCall);
                }
            }
            catch (Exception e)
            {
                Log(string.Format("In Dynamic Proxy- Exception {0} executing '{1}'", e, methodCall.MethodName));
                return new ReturnMessage(e, methodCall);
            }
        }

        private IInterceptor GetIInterceptor(MethodInfo methodInfo)
        {
            IInterceptor interceptor=null;
            var type = methodInfo.DeclaringType;
            var attribute = methodInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (attribute != null)
            {

                //判断是否认证逻辑
                interceptor = new AuthorizeInterceptor(attribute);
            }
            return interceptor;
        }
    }
}
