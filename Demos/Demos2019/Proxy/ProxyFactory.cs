using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Proxy
{
    //public class ProxyFactory
    //{
    //    //public static IProxy Create()
    //    //{
    //    //    var proxy = new Proxy();
    //    //    var dynamicProxy = new DynamicProxy<IProxy>(proxy);
    //    //    return dynamicProxy.GetTransparentProxy() as IProxy;
    //    //}

    //    //public static T Create<T>() where T : class, new()
    //    //{
    //    //    var t = new T();
    //    //    var dynamicProxy = new DynamicProxy<T>(t);
    //    //    //上面两句如果不想每次都创建，可以用池的缓存来解决。
    //    //    return dynamicProxy.GetTransparentProxy() as T;
    //    //}
    //}

    public class ProxyFactory<T> where T : class, new()
    {
        //public static IProxy Create()
        //{
        //    var proxy = new Proxy();
        //    var dynamicProxy = new DynamicProxy<IProxy>(proxy);
        //    return dynamicProxy.GetTransparentProxy() as IProxy;
        //}

        public static T Create() 
        {
            var t = new T();
            var dynamicProxy = new DynamicProxy<T>(t);
            //上面两句如果不想每次都创建，可以用池的缓存来解决。
            return dynamicProxy.GetTransparentProxy() as T;
        }
    }
}
