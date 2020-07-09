using Demos.OpenResource.DotNettyDemo.Echo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo
{
    public class NettyTest
    {
        /*
         * Netty基础程序集：DotNetty.Buffers、DotNetty.Codecs、DotNetty.Common
         *         DotNetty.Handlers、DotNetty.Transport、DotNetty.Transport.Libuv（可选）
         *         
         * 如果使用WebSocket要添加：DotNetty.Codecs.Http
         */
        public void Test()
        {
            /*
             * 参照GitHub下载的源码中Sample中Echo.Client项目的Netty引用
             * 安装引用Dll
             * DotNetty.Buffers、DotNetty.Codecs、DotNetty.Common、DotNetty.Handlers、DotNetty.Transport、DotNetty.Transport.Libuv
             * */

            //本身就是异步方法，不必再额外创建线程
            //Task.Run(() =>
            //{
            //    new EchoNettyServer().RunServerAsync().Wait();
            //});
            //使用Wait会阻塞下面的方法，除非放在线程中执行
            new EchoNettyServer().RunServerAsync();//.Wait();
            Thread.Sleep(2000);
            EchoNettyClient echoNettyClient = new EchoNettyClient();
            echoNettyClient.RunClientAsync();//.Wait();
            Thread.Sleep(12001);
            echoNettyClient.Stop();
            //Task.Run(() =>
            //{
            //    new EchoNettyClient().RunClientAsync().Wait();
            //});


        }
    }
}
