﻿using Demos.Model;
using Demos.OpenResource.DotNettyDemo.Model;
using Demos.OpenResource.DotNettyDemo.Protobuf;
using DotNetty.Codecs;
using DotNetty.Codecs.Protobuf;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.opensource.protobuf.model;

namespace Demos.OpenResource.DotNettyDemo.Echo
{
    public class EchoNettyServer
    {
        /*
         * NuGet: DotNetty.Codecs.Protobuf :ProtobufDecoder
         */


        IChannel boundChannel;
        IEventLoopGroup bossGroup;
        IEventLoopGroup workerGroup;
        public async Task RunServerAsync()
        {
            //ExampleHelper.SetConsoleLogger();

            //libuv是一个高性能的，事件驱动的I/O库，并且提供了跨平台（如windows, linux）的API。
            //将Dll-->Netty下的libuv.dll复制到运行目录
            //Echo没有采用Libuv，调试注意
            var useLibuv = false;
            if (useLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                bossGroup = dispatcher;
                workerGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                bossGroup = new MultithreadEventLoopGroup(1);
                workerGroup = new MultithreadEventLoopGroup();
            }
            //客户端和服务端都是用加密，否则收不到数据
            //X509Certificate2 tlsCertificate = null;
            //if (ServerSettings.IsSsl)
            //{
            //    tlsCertificate = new X509Certificate2(Path.Combine(ExampleHelper.ProcessDirectory, "dotnetty.com.pfx"), "password");
            //}
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);

                if (useLibuv)
                {
                    bootstrap.Channel<TcpServerChannel>();
                }
                else
                {
                    bootstrap.Channel<TcpServerSocketChannel>();
                }

                bootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    //.Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        //if (tlsCertificate != null)
                        //{
                        //    pipeline.AddLast("tls", TlsHandler.Server(tlsCertificate));
                        //}
                        //pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        IdleStateHandler idleStateHandler = new IdleStateHandler(2, 2, 6);

                        pipeline.AddLast("timeout", idleStateHandler);
                        //框架解码器：防止TCP粘包。 FixedLengthFrameDecoder、LineBasedFrameDecoder、DelimiterBasedFrameDecoder和LengthFieldBasedFrameDecoder
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        //pipeline.AddLast("StringDecoder", new StringDecoder());
                        //pipeline.AddLast("StringEncoder", new StringEncoder());

                        //pipeline.AddLast("ProtobufDecoder", new ProtobufDecoder(PersonProto.Parser));
                        //pipeline.AddLast("ProtobufEncoder", new ProtobufEncoder());


                        pipeline.AddLast("ObjectDecoder", new ObjectDecoder<Person>());
                        pipeline.AddLast("ObjectEncoder", new ObjectEncoder());



                        //StringDecoder
                        //StringEncoder
                        //var en = new ProtobufEncoder();
                        //var de = new ProtobufDecoder(PersonProto.Parser);
                        pipeline.AddLast("echo", new EchoServerHandler());
                    }));

                Console.WriteLine($"listen port - {8031}");
                boundChannel = await bootstrap.BindAsync(8031);
                ////防止通道关闭，生产环境不会执行下面的CloseAsync();，会在一个Stop方法中调用
                //Console.ReadLine();

                //await boundChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //    await Task.WhenAll(
                //        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                //        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                //}
            }
        }

        public async void Close()
        {

            await boundChannel.CloseAsync();
            await Task.WhenAll(
        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));


        }
    }
}
