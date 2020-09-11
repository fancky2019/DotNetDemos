using DotNetty.Codecs.Http;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.WebSocket
{
    public class WebSocketsServer
    {
        /*
        * 客户端测试网页：在DW项目的websocketdemo.html。
        * 启动WebSocket服务程序：监听8031端口
        */

        #region 客户端测试相关代码
        /*
         * 浏览器控制台测试连接
         * 在浏览器控制台执行：
         *    //WebSocket("ws://127.0.0.1:8888/websocket?token=tokendata");
         * var ws = new WebSocket("ws://127.0.0.1:8031/");
            ws.onopen = function() { 
                ws.send('websocekt测试'); 
            };
            ws.onmessage = function(e) {
                alert("收到服务端的消息：" + e.data);
            };




          // 客户端网页js代码
            $(function () {

                var inc = document.getElementById('incomming');
                var input = document.getElementById('sendText');
                inc.innerHTML += "connecting to server ..<br/>";

                // create a new websocket and connect
                // window.ws = new wsImpl('ws://127.0.0.1:8031/');
                let ws=null;
                $('#btnConnect').on('click', function () {
                   // create a new websocket and connect
                     ws=new WebSocket('ws://127.0.0.1:8031/');
                    // when data is comming from the server, this metod is called
                    ws.onmessage = function (evt) {
                        inc.innerHTML += evt.data + '<br/>';
                    };

                    // when the connection is established, this method is called
                    ws.onopen = function () {
                        inc.innerHTML += '.. connection open<br/>';
                    };

                    // when the connection is closed, this method is called
                    ws.onclose = function () {
                        inc.innerHTML += '.. connection closed<br/>';
                    }
                });


                $('#btnSend').on('click', function () {
                 var val = input.value;
                if(!window.WebSocket||ws==null){return;}
                if(ws.readyState == WebSocket.OPEN){
                    ws.send(message);
                }else{
                    alert("WebSocket 连接没有建立成功！");
                }
                });

                $('#btnDisconnect').on('click', function () {
                    ws.close();
                });
            });
         */
        #endregion


        IChannel _bootstrapChannel;
        IEventLoopGroup _bossGroup;
        IEventLoopGroup _workGroup;
        public  async Task RunServerAsync()
        {
            Console.WriteLine(
                $"\n{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}"
                + $"\n{RuntimeInformation.ProcessArchitecture} {RuntimeInformation.FrameworkDescription}"
                + $"\nProcessor Count : {Environment.ProcessorCount}\n");

            bool useLibuv = true;// ServerSettings.UseLibuv;
            Console.WriteLine("Server Transport type : " + (useLibuv ? "Libuv" : "Socket"));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            }

            Console.WriteLine($"Server garbage collection : {(GCSettings.IsServerGC ? "Enabled" : "Disabled")}");
            Console.WriteLine($"Current latency mode for garbage collection: {GCSettings.LatencyMode}");
            Console.WriteLine("\n");


            if (useLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                _bossGroup = dispatcher;
                _workGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                _bossGroup = new MultithreadEventLoopGroup(1);
                _workGroup = new MultithreadEventLoopGroup();
            }

            //X509Certificate2 tlsCertificate = null;
            //if (ServerSettings.IsSsl)
            //{
            //    tlsCertificate = new X509Certificate2(Path.Combine(ExampleHelper.ProcessDirectory, "dotnetty.com.pfx"), "password");
            //}
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(_bossGroup, _workGroup);

                if (useLibuv)
                {
                    bootstrap.Channel<TcpServerChannel>();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        bootstrap
                            .Option(ChannelOption.SoReuseport, true)
                            .ChildOption(ChannelOption.SoReuseaddr, true);
                    }
                }
                else
                {
                    bootstrap.Channel<TcpServerSocketChannel>();
                }

                bootstrap
                    .Option(ChannelOption.SoBacklog, 8192)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        //if (tlsCertificate != null)
                        //{
                        //    pipeline.AddLast(TlsHandler.Server(tlsCertificate));
                        //}
                        pipeline.AddLast(new HttpServerCodec());
                        pipeline.AddLast(new HttpObjectAggregator(65536));
                        pipeline.AddLast(new WebSocketServerHandler());
                    }));

                int port = 8031;
                //指定环回地址，只能监听127.不能使用ip
                //IChannel bootstrapChannel = await bootstrap.BindAsync(IPAddress.Loopback, port);
                 _bootstrapChannel = await bootstrap.BindAsync(port);
                Console.WriteLine("Open your web browser and navigate to "
                    + "http"
                    + $"://127.0.0.1:{port}/");
                Console.WriteLine("Listening on "
                    + "ws"
                    + $"://127.0.0.1:{port}/websocket");


                //await _bootstrapChannel.CloseAsync();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                //_workGroup.ShutdownGracefullyAsync().Wait();
                //_bossGroup.ShutdownGracefullyAsync().Wait();
            }
        }

        public void Close()
        {
             _bootstrapChannel?.CloseAsync().Wait();
            _workGroup?.ShutdownGracefullyAsync().Wait();
            _bossGroup?.ShutdownGracefullyAsync().Wait();
        }
    }
}
