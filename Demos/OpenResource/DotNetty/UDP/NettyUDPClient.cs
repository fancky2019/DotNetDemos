
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNetty.UDP
{
    public class NettyUDPClient
    {
        public void Test()
        {
            Task.Run(() =>
            {

                //Client();
                MulticastClient();
            });

        }

        #region 单播,广播
        /// <summary>
        /// 可以收到服务端单播、广播信息，收不到多播
        /// </summary>
        public async void Client()
        {
            Bootstrap bootstrap = new Bootstrap();
            var group = new MultithreadEventLoopGroup();
            try
            {

                bootstrap.Group(group);
                bootstrap.Channel<SocketDatagramChannel>();
                bootstrap.Handler(new NettyUDPClientHnadler());
                SocketDatagramChannel channel = (SocketDatagramChannel)bootstrap.BindAsync(6000).Result;
                //task.Result.CloseAsync();
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

        #endregion

        #region 单播、多播、广播
        /// <summary>
        ///  可以收到服务端单播、多播、广播信息
        /// </summary>
        private async void MulticastClient()
        {
            Bootstrap bootstrap = new Bootstrap();
            var group = new MultithreadEventLoopGroup();
            try
            {
                var clientBootstrap = new Bootstrap();
                clientBootstrap
                    .Group(group)
                    //多播要指定寻址方案为IPV4
                    .ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork))
                    .Handler(new NettyUDPClientHnadler());


                IChannel channel = clientBootstrap.BindAsync(6000).Result;

                SocketDatagramChannel serverChannel = (SocketDatagramChannel)channel;
                //var serverEndPoint = (IPEndPoint)serverChannel.LocalAddress;

                IPAddress multicastAddress = IPAddress.Parse("225.0.0.1");
                var groupAddress = new IPEndPoint(multicastAddress, 6000);
                Task joinTask = serverChannel.JoinGroup(groupAddress);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                //Task leaveTask = serverChannel.LeaveGroup(groupAddress, loopback);
                //await group.ShutdownGracefullyAsync();
            }
        }
        #endregion

        #region  github 多播单元测试
        private NetworkInterface LoopbackInterface(AddressFamily addressFamily)
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (addressFamily == AddressFamily.InterNetwork)
            {
                return networkInterfaces[NetworkInterface.LoopbackInterfaceIndex];
            }

            if (addressFamily == AddressFamily.InterNetworkV6)
            {
                return networkInterfaces[NetworkInterface.IPv6LoopbackInterfaceIndex];
            }

            throw new NotSupportedException($"Address family {addressFamily} is not supported. Expecting InterNetwork/InterNetworkV6");
        }



        private void MulticastClinetServer()
        {
            SocketDatagramChannel serverChannel = null;
            IChannel clientChannel = null;
            var serverGroup = new MultithreadEventLoopGroup(1);
            var clientGroup = new MultithreadEventLoopGroup(1);
            NetworkInterface loopback = LoopbackInterface(AddressFamily.InterNetwork);

            //IPAddress address = IPAddress.Loopback;
            try
            {
                var multicastHandler = new NettyUDPServerHnadler();
                var serverBootstrap = new Bootstrap();
                serverBootstrap
                    .Group(serverGroup)
                    .ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork))
                    //.Option(ChannelOption.Allocator, allocator)
                    //.Option(ChannelOption.SoReuseaddr, true)
                    //.Option(ChannelOption.IpMulticastLoopDisabled, false)
                    .Handler(multicastHandler);


                Task<IChannel> task = serverBootstrap.BindAsync(IPAddress.Parse("192.168.1.114"), 6000);

                serverChannel = (SocketDatagramChannel)task.Result;
                var serverEndPoint = (IPEndPoint)serverChannel.LocalAddress;




                var clientBootstrap = new Bootstrap();
                clientBootstrap
                    .Group(clientGroup)
                    .ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork))
                    //.Option(ChannelOption.Allocator, allocator)
                    //.Option(ChannelOption.SoReuseaddr, true)
                    //.Option(ChannelOption.IpMulticastLoopDisabled, false)
                    .Handler(new NettyUDPClientHnadler());


                task = clientBootstrap.BindAsync(IPAddress.Parse("192.168.1.114"), 6001);

                clientChannel = (SocketDatagramChannel)task.Result;

                IPAddress multicastAddress = IPAddress.Parse("230.0.0.1");//: IPAddress.Parse("ff12::1");
                var groupAddress = new IPEndPoint(multicastAddress, 6000);
                Task joinTask = serverChannel.JoinGroup(groupAddress);

                clientChannel.WriteAndFlushAsync(new DatagramPacket(Unpooled.Buffer().WriteBytes(
                    Encoding.UTF8.GetBytes("Hello world")), groupAddress)).Wait();

                //Task leaveTask = serverChannel.LeaveGroup(groupAddress, loopback);

                // sleep half a second to make sure we left the group
                Task.Delay(1000).Wait();

                // we should not receive a message anymore as we left the group before
                //clientChannel.WriteAndFlushAsync(new DatagramPacket(Unpooled.Buffer().WriteInt(1), groupAddress)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //serverChannel?.CloseAsync().Wait(TimeSpan.FromMilliseconds(1000));
                //clientChannel?.CloseAsync().Wait(TimeSpan.FromMilliseconds(1000));

                //Task.WaitAll(
                //    serverGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                //    clientGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }
        #endregion

    }
}
