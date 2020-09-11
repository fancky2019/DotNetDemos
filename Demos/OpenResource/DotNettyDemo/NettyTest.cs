using Demos.OpenResource.DotNettyDemo.Echo;
using Demos.OpenResource.DotNettyDemo.WebSocket;
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
            //new EchoNettyServer().RunServerAsync().Wait();

            //EchoNettyClient echoNettyClient = new EchoNettyClient();
            //echoNettyClient.RunClientAsync().Wait();


            //echoNettyClient.SendMsg();

            //echoNettyClient.Stop();
            //Task.Run(() =>
            //{
            //    new EchoNettyClient().RunClientAsync().Wait();
            //});

            WebSocketTest();
        }

        public void WebSocketTest()
        {
            /*
             * 客户端测试网页：在DW项目的websocketdemo.html。
             * 启动WebSocket服务程序：监听8031端口
             */

            #region 客户端测试相关代码
            /*
             * 浏览器控制台测试连接
             * 在浏览器控制台执行：
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


            //启动WebSocket服务端
            Task.Run(() =>
            {
                new WebSocketsServer().RunServerAsync().Wait();
            });

            //Thread.Sleep(2000);
            ////启动WebSocket服务端
            //Task.Run(() =>
            //{
            //    WebSocketClient webSocketClient = new WebSocketClient();
            //    webSocketClient.RunClientAsync().Wait();
            //    //var sendResult = webSocketClient.SendMessage();

            //    //socket连接成功，还要握手成功，才能发送消息
            //    if (!webSocketClient.HandshakeComplete)
            //    {
            //        DateTime dateTime1 = DateTime.Now;
            //        while (!webSocketClient.HandshakeComplete)
            //        {
            //            new SpinWait().SpinOnce();
            //        }
            //        DateTime dateTime2 = DateTime.Now;

            //        var duration = dateTime2 - dateTime1;
            //        var mills = duration.TotalMilliseconds;
            //        Console.WriteLine($"mills:{mills}");
            //        webSocketClient.SendMessage();
            //    }
            //    Thread.Sleep(5000);
            //    webSocketClient.Close();
            //});

        }
    }
}
