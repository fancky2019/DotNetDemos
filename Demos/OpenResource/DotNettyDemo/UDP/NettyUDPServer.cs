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

namespace Demos.OpenResource.DotNettyDemo.UDP
{
    public class NettyUDPServer
    {
        public void Test()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                //Server();
                MulticastServer();
            });

        }

        public async void Server()
        {
            Bootstrap bootstrap = new Bootstrap();
            var clientGroup = new MultithreadEventLoopGroup(1);
            try
            {

                bootstrap.Group(clientGroup);
                bootstrap.ChannelFactory(() => new SocketDatagramChannel());
                //多播要指定寻址方案为IPV4
                //bootstrap.ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork));
                bootstrap.Handler(new NettyUDPClientHnadler());
                //IChannel chanel = bootstrap.BindAsync(IPAddress.Parse("192.168.1.114"), 6001).Result;
                IChannel chanel = bootstrap.BindAsync(6001).Result;
                //向目标端口发送信息

                //单
                await chanel.WriteAndFlushAsync(new DatagramPacket(
                         Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("单播信息")),
                        new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000)));

                //广播
                //await chanel.WriteAndFlushAsync(new DatagramPacket(
                //      Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("广播信息")),
                //     new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6000)));
                await chanel.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                await clientGroup.ShutdownGracefullyAsync();
            }
        }

        public async void MulticastServer()
        {
            Bootstrap bootstrap = new Bootstrap();
            var clientGroup = new MultithreadEventLoopGroup(1);
            try
            {

                bootstrap.Group(clientGroup);
                //多播要指定寻址方案为IPV4
                bootstrap.ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork));
                bootstrap.Handler(new NettyUDPClientHnadler());
                //IChannel chanel = bootstrap.BindAsync(IPAddress.Parse("192.168.1.114"), 6001).Result;
                IChannel chanel = bootstrap.BindAsync(6001).Result;
                //向目标端口发送信息

                //多播
                chanel.WriteAndFlushAsync(new DatagramPacket(
                        Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes("多播信息")),
                       new IPEndPoint(IPAddress.Parse("225.0.0.1"), 6000))).Wait();


                await chanel.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                await clientGroup.ShutdownGracefullyAsync();
            }
        }
    }
}
