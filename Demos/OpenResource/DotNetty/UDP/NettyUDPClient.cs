
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
                //MulticastClient();
                Multicast();
            });

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                //Client();
                T();
                //Multicast();
            });

        }

        #region 单播,广播
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

        private async void MulticastClient()
        {
            Bootstrap bootstrap = new Bootstrap();
            var group = new MultithreadEventLoopGroup();
            try
            {
                NetworkInterface ni = LoopbackInterface(AddressFamily.InterNetwork);
                bootstrap.Group(group);
                bootstrap.ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork));


                bootstrap.Handler(new NettyUDPClientHnadler());
                SocketDatagramChannel channel = (SocketDatagramChannel)bootstrap.BindAsync(6000).Result;
                //task.Result.CloseAsync();
                IPEndPoint multicastAddress = new IPEndPoint(IPAddress.Parse("225.0.0.1"), 6000);
                await channel.JoinGroup(multicastAddress, ni);
                //Task leaveTask = serverChannel.LeaveGroup(groupAddress, loopback);
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
        //region 多播：可以接受单播、多播、广播信息
        //        private void runMulticastClient() throws InterruptedException
        //        {
        //            EventLoopGroup workerGroup = new NioEventLoopGroup();
        //        try {

        //            NetworkInterface ni = NetUtil.LOOPBACK_IF;
        //        // InetSocketAddress相当于c#IPEndPoint
        //        InetSocketAddress multicastAddress = new InetSocketAddress(InetAddress.getByName("225.0.0.1"), 6000);

        //        Bootstrap bootstrap = new Bootstrap(); // (1)
        //        bootstrap.group(workerGroup); // (2)
        //            bootstrap.channel(NioDatagramChannel.class);
        //            bootstrap.handler(new NettyUDPClientHandler());

        //           //加入多播组：和单播多播区别就是：加入了多播组。但是可以收到单播，多播的信息。
        //            NioDatagramChannel ch = (NioDatagramChannel)bootstrap.bind(multicastAddress.getPort()).sync().channel();
        //        ch.joinGroup(multicastAddress, ni).sync();

        //        ch.closeFuture().await();

        //    } catch (Exception ex) {
        //            System.out.println(ex.toString());
        //} finally {
        //            workerGroup.shutdownGracefully();
        //        }
        //    }
        //endregion


        public NetworkInterface LoopbackInterface(AddressFamily addressFamily)
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



        public void Multicast()
        {
            SocketDatagramChannel serverChannel = null;
            IChannel clientChannel = null;
            var serverGroup = new MultithreadEventLoopGroup(1);

            NetworkInterface loopback = LoopbackInterface(AddressFamily.InterNetwork);

            try
            {

                var serverBootstrap = new Bootstrap();
                serverBootstrap
                    .Group(serverGroup)
                    .ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork))
                    //.Option(ChannelOption.Allocator, allocator)
                    .Option(ChannelOption.SoReuseaddr, true)
                    .Option(ChannelOption.IpMulticastLoopDisabled, false)
                    .Handler(new NettyUDPClientHnadler());

                IPAddress address = IPAddress.Loopback;

                Task<IChannel> task = serverBootstrap.BindAsync(6000);

                serverChannel = (SocketDatagramChannel)task.Result;
                var serverEndPoint = (IPEndPoint)serverChannel.LocalAddress;


                IPAddress multicastAddress =
              IPAddress.Parse("225.0.0.1");
                var groupAddress = new IPEndPoint(multicastAddress, 6000);
                Task joinTask = serverChannel.JoinGroup(groupAddress, loopback);
            }
            finally
            {
                serverChannel?.CloseAsync().Wait(TimeSpan.FromMilliseconds(1000));
                clientChannel?.CloseAsync().Wait(TimeSpan.FromMilliseconds(1000));

                Task.WaitAll(
                    serverGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        private void T()
        {
            var clientGroup = new MultithreadEventLoopGroup(1);
            var clientBootstrap = new Bootstrap();
            clientBootstrap
                .Group(clientGroup)
                .ChannelFactory(() => new SocketDatagramChannel(AddressFamily.InterNetwork))
                //.Option(ChannelOption.Allocator, allocator)
                .Option(ChannelOption.SoReuseaddr, true)
                .Option(ChannelOption.IpMulticastLoopDisabled, false)
                .Handler(new NettyUDPServerHnadler());


            IPAddress address = IPAddress.Loopback;
            Task<IChannel> task = clientBootstrap.BindAsync(address,6001);
            
            SocketDatagramChannel clientChannel = (SocketDatagramChannel)task.Result;

            IPAddress multicastAddress =
                IPAddress.Parse("225.0.0.1");
            var groupAddress = new IPEndPoint(multicastAddress, 6000);
            //Task joinTask = serverChannel.JoinGroup(groupAddress, loopback);

            clientChannel.WriteAndFlushAsync(new DatagramPacket(Unpooled.Buffer().WriteInt(1), groupAddress)).Wait();

            //    Task leaveTask = serverChannel.LeaveGroup(groupAddress, loopback);

            // sleep half a second to make sure we left the group
            Task.Delay(1000).Wait();

            // we should not receive a message anymore as we left the group before
            clientChannel.WriteAndFlushAsync(new DatagramPacket(Unpooled.Buffer().WriteInt(1), groupAddress)).Wait();

        }

    }
}
