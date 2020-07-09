using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Echo
{

    public class EchoNettyClient
    {
        Bootstrap _bootstrap = new Bootstrap();
        IChannel _clientChannel = null;
        string _ip = "192.168.1.105";
        string _port = "8031";
        IPEndPoint _iPEndPoint = null;
        public async Task RunClientAsync()
        {
            _iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), int.Parse(_port));
            //ExampleHelper.SetConsoleLogger();

            var group = new MultithreadEventLoopGroup();

            //X509Certificate2 cert = null;
            string targetHost = null;
            //if (ClientSettings.IsSsl)
            //{
            //    cert = new X509Certificate2(Path.Combine(ExampleHelper.ProcessDirectory, "dotnetty.com.pfx"), "password");
            //    targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            //}
            try
            {
                _bootstrap.Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        //if (cert != null)
                        //{
                        //    pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                        //}
                        //pipeline.AddLast(new LoggingHandler());
                        //6s未读写就断开了连接。和java的一样设计
                        IdleStateHandler idleStateHandler = new IdleStateHandler(2, 2, 6);

                        pipeline.AddLast("timeout", idleStateHandler);
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        EchoClientHandler echoClientHandler = new EchoClientHandler();
                        echoClientHandler.DisConnected += () =>
                          {
                              Console.WriteLine("尝试重新建立连接......");
                              //最长等待三次读写时间6秒，若仍没有建立连接进行读写，继续回调此事件
                              Connect(_iPEndPoint).Wait(6);


                              //while (!Connect(_iPEndPoint).Wait(2))
                              //{
                              //    //两秒内未连接继续尝试连接
                              //}

                          };
                        pipeline.AddLast("echo", echoClientHandler);
                    }));

                _clientChannel = await Connect(_iPEndPoint);

                //防止通道关闭，生产环境不会执行下面的CloseAsync();，会在一个Stop方法中调用
                Console.ReadLine();

            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        private async Task<IChannel> Connect(IPEndPoint iPEndPoint)
        {
            return await _bootstrap.ConnectAsync(iPEndPoint);
        }

        public void Stop()
        {
            _clientChannel.CloseAsync().Wait();
        }


    }
}
