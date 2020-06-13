using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNetty.UDP
{
    public class NettyUDPServer
    {
        public void Test()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Server();
            });

        }

        public async void Server()
        {
            Bootstrap bootstrap = new Bootstrap();
            var group = new MultithreadEventLoopGroup();
            try
            {

                bootstrap.Group(group);
                bootstrap.ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork));

                bootstrap.Handler(new NettyUDPClientHnadler());
                IChannel chanel = bootstrap.BindAsync(6001).Result;

                //向目标端口发送信息

                //单
                //await chanel.WriteAndFlushAsync(new DatagramPacket(
                //         Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("单播信息")),
                //        new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000)));

                //广播
                //await chanel.WriteAndFlushAsync(new DatagramPacket(
                //      Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("广播信息")),
                //     new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6000)));

                await chanel.WriteAndFlushAsync(new DatagramPacket(
             Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("多播信息")),
            new IPEndPoint(IPAddress.Parse("255.0.0.1"), 6000)));

                await chanel.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                await group.ShutdownGracefullyAsync();
            }
        }
    }
}
