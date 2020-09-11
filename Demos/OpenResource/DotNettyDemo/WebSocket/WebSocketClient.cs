using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Codecs.Http.WebSockets.Extensions.Compression;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.WebSocket
{
    /*
     * 在Netty基础上
     * Nuget安装DotNetty.Codecs.Http
     */
    public class WebSocketClient
    {

        IChannel _ch = null;
        IEventLoopGroup _group;
        public bool HandshakeComplete { get; private set; }
        public async Task RunClientAsync()
        {
            var host = "127.0.0.1";
            var port = 8031;
            var isSsl = false;
            var builder = new UriBuilder
            {
                Scheme = isSsl ? "wss" : "ws",
                Host = host,
                Port = port
            };

            //WebSocket("ws://127.0.0.1:8888/websocket?token=tokendata");
            string path = "websocket";
            if (!string.IsNullOrEmpty(path))
            {
                builder.Path = path;
            }

            Uri uri = builder.Uri;


            bool useLibuv = true;// ClientSettings.UseLibuv;
            Console.WriteLine("Client Transport type : " + (useLibuv ? "Libuv" : "Socket"));


            if (useLibuv)
            {
                _group = new EventLoopGroup();
            }
            else
            {
                _group = new MultithreadEventLoopGroup();
            }


            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(_group)
                    .Option(ChannelOption.TcpNodelay, true);
                if (useLibuv)
                {
                    bootstrap.Channel<TcpChannel>();
                }
                else
                {
                    bootstrap.Channel<TcpSocketChannel>();
                }

                // Connect with V13 (RFC 6455 aka HyBi-17). You can change it to V08 or V00.
                // If you change it to V00, ping is not supported and remember to change
                // HttpResponseDecoder to WebSocketHttpResponseDecoder in the pipeline.


                var handler = new WebSocketClientHandler(
                    WebSocketClientHandshakerFactory.NewHandshaker(
                            uri, WebSocketVersion.V13, null, true, new DefaultHttpHeaders()));

                handler.HandshakeComplete += () => this.HandshakeComplete = true;
                bootstrap.Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;


                    pipeline.AddLast(
                        new HttpClientCodec(),
                        new HttpObjectAggregator(8192),
                        WebSocketClientCompressionHandler.Instance,
                        handler);
                }));

                //IChannel ch = await bootstrap.ConnectAsync(new IPEndPoint(ClientSettings.Host, ClientSettings.Port));
                _ch = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(host), port));
                await handler.HandshakeCompletion;



                //await _ch.CloseAsync();

            }
            catch (Exception ex)
            {
                //Console.ReadLine();
            }
            finally
            {
                //await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// socket连接成功，还要握手成功，才能发送消息
        /// </summary>
        /// <returns></returns>
        public bool SendMessage()
        {
            if (!this.HandshakeComplete)
            {
                Console.WriteLine("Handshake is not completed.");
                return false;
            }
            string msg = "websocket test";
            //WebSocketFrame frame = new TextWebSocketFrame(msg);




            var buffer = Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes(msg));
            WebSocketFrame frame = new TextWebSocketFrame(buffer);



            //string msg = "websocket test";
            //IByteBuffer initialMessage = Unpooled.Buffer(1024);
            //byte[] messageBytes = Encoding.UTF8.GetBytes(msg);
            //initialMessage.WriteBytes(messageBytes);
            //WebSocketFrame frame = new TextWebSocketFrame(initialMessage);



            _ch.WriteAndFlushAsync(frame).Wait();
            Console.WriteLine($"Client sended :{msg}");
            return true;
        }
        public async void Close()
        {
            await _ch.CloseAsync();
            await _group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }


    }
}
