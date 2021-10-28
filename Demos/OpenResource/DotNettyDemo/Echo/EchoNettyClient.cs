using DotNetty.Codecs;
using DotNetty.Codecs.Protobuf;
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
using Test.opensource.protobuf.model;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Demos.OpenResource.DotNettyDemo.Protobuf;
using Demos.Model;
using Demos.OpenResource.DotNettyDemo.Model;

namespace Demos.OpenResource.DotNettyDemo.Echo
{

    public class EchoNettyClient
    {
        Bootstrap _bootstrap = new Bootstrap();
        IChannel _clientChannel = null;
        string _ip = "127.0.0.1";
        //string _ip = "192.168.1.114";
        string _port = "8031";
        IPEndPoint _iPEndPoint = null;
        MultithreadEventLoopGroup group;
        public async Task RunClientAsync()
        {
            _iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), int.Parse(_port));
            //ExampleHelper.SetConsoleLogger();

            group = new MultithreadEventLoopGroup();

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
                        //框架解码器：防止TCP粘包。 FixedLengthFrameDecoder、LineBasedFrameDecoder、DelimiterBasedFrameDecoder和LengthFieldBasedFrameDecoder
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        //pipeline.AddLast("StringDecoder", new StringDecoder());
                        //pipeline.AddLast("StringEncoder", new StringEncoder());

                        //pipeline.AddLast("ProtobufDecoder", new ProtobufDecoder(PersonProto.Parser));
                        //pipeline.AddLast("ProtobufEncoder", new ProtobufEncoder());


                        pipeline.AddLast("ObjectDecoder", new ObjectDecoder<Person>());
                        pipeline.AddLast("ObjectEncoder", new ObjectEncoder());

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

                ////防止通道关闭，生产环境不会执行下面的CloseAsync();，会在一个Stop方法中调用
                //Console.ReadLine();
                //_clientChannel.CloseAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //    await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        public void SendMsg()
        {

            //string msg = " Client sended  msg";
            //_clientChannel.WriteAndFlushAsync(msg);


            Person data = new Person
            {
                Name = "rui",
                Age = 6
            };

            _clientChannel.WriteAndFlushAsync(data);

            //MessageInfo messageInfo = new MessageInfo() { MessageType = MessageType.HeartBeat };
            //_clientChannel.WriteAndFlushAsync(messageInfo);


            //Job job = new Job() { Name = "chengxuyuan", Salary = 700 };
            //Job job1 = new Job() { Name = "nongmin", Salary = 700 };
            //Any any = Any.Pack(job);
            //PersonProto personProto = new PersonProto()
            //{
            //    Id = 1,
            //    Name = "fancky",
            //    Age = 27,
            //    Gender = Gender.Man
            //};

            //personProto.Sons.Add("li");
            //personProto.Sons.Add("fa");
            //personProto.Any.Add(any);
            //personProto.Jobs.Add(job);
            //personProto.SonJobs.Add("li", job);
            //personProto.SonJobs.Add("fa", job1);

            //_clientChannel.WriteAndFlushAsync(personProto);



        }

        private async Task<IChannel> Connect(IPEndPoint iPEndPoint)
        {
            try
            {


                return await _bootstrap.ConnectAsync(iPEndPoint);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void Stop()
        {
            _clientChannel.CloseAsync().Wait();
            await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));

        }


    }
}
