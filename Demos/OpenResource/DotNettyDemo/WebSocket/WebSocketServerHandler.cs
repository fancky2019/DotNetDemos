﻿using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//官方Demo使用了静态类
using static DotNetty.Codecs.Http.HttpVersion;
using static DotNetty.Codecs.Http.HttpResponseStatus;

namespace Demos.OpenResource.DotNettyDemo.WebSocket
{
    public class WebSocketServerHandler : SimpleChannelInboundHandler<object>
    {



        const string WebsocketPath = "/websocket";

        WebSocketServerHandshaker handshaker;

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine($"Client - {context.Channel.RemoteAddress.ToString()} connected。");
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine($"Client - {context.Channel.RemoteAddress.ToString()} disconnected。");
            base.ChannelInactive(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {

            if (msg is IFullHttpRequest request)//websocket的http握手连接过程
            {
                this.HandleHttpRequest(ctx, request);
            }
            else if (msg is WebSocketFrame frame)
            {
                this.HandleWebSocketFrame(ctx, frame);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        void HandleHttpRequest(IChannelHandlerContext ctx, IFullHttpRequest req)
        {
            //   //WebSocket("ws://127.0.0.1:8888/ws?token=tokendata");
            ///ws?token=tokendata
            var uri = req.Uri;
            if (uri.Contains("token="))
            {
                var tokenIndex = uri.IndexOf("token=");
                var token = uri.Substring(tokenIndex + 6);
            }
            // Handle a bad request.
            if (!req.Result.IsSuccess)
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, BadRequest));
                return;
            }

            // Allow only GET methods.
            if (!Equals(req.Method, HttpMethod.Get))
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, Forbidden));
                return;
            }

            // Send the demo page and favicon.ico
            //if ("/".Equals(req.Uri))
            //{
            //    IByteBuffer content = Unpooled.WrappedBuffer(
            //    Encoding.ASCII.GetBytes("已连接!")); ;// WebSocketServerBenchmarkPage.GetContent(GetWebSocketLocation(req));
            //    var res = new DefaultFullHttpResponse(Http11, OK, content);

            //    res.Headers.Set(HttpHeaderNames.ContentType, "text/html; charset=UTF-8");
            //    HttpUtil.SetContentLength(res, content.ReadableBytes);

            //    SendHttpResponse(ctx, req, res);
            //    return;
            //}
            if ("/favicon.ico".Equals(req.Uri))
            {
                var res = new DefaultFullHttpResponse(Http11, NotFound);
                SendHttpResponse(ctx, req, res);
                return;
            }

            //响应客户端的握手
            // Handshake
            var wsFactory = new WebSocketServerHandshakerFactory(
                GetWebSocketLocation(req), null, true, 5 * 1024 * 1024);
            this.handshaker = wsFactory.NewHandshaker(req);
            if (this.handshaker == null)
            {
                WebSocketServerHandshakerFactory.SendUnsupportedVersionResponse(ctx.Channel);
            }
            else
            {
                this.handshaker.HandshakeAsync(ctx.Channel, req);
            }
        }

        void HandleWebSocketFrame(IChannelHandlerContext ctx, WebSocketFrame frame)
        {
            // Check for closing frame
            if (frame is CloseWebSocketFrame)
            {
                this.handshaker.CloseAsync(ctx.Channel, (CloseWebSocketFrame)frame.Retain());
                return;
            }

            if (frame is PingWebSocketFrame)
            {
                ctx.WriteAsync(new PongWebSocketFrame((IByteBuffer)frame.Content.Retain()));
                return;
            }

            if (frame is TextWebSocketFrame textWebSocketFrame)
            {
                //接收到来自客户端的字符串消息
                var reveivedMsg = textWebSocketFrame.Text();
                var msg = $"服务端已收到客户端消息:{reveivedMsg}";
                Console.WriteLine(msg);
                // Echo the frame
                //ctx.WriteAsync(frame.Retain());
                //返回客户端信息，参考java 的netty 的websocket sample
                ctx.WriteAsync(new TextWebSocketFrame(msg));
                return;
            }

            if (frame is BinaryWebSocketFrame)
            {
                // Echo the frame
                ctx.WriteAsync(frame.Retain());
            }
        }

        void SendHttpResponse(IChannelHandlerContext ctx, IFullHttpRequest req, IFullHttpResponse res)
        {
            // Generate an error page if response getStatus code is not OK (200).
            if (res.Status.Code != 200)
            {
                IByteBuffer buf = Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes(res.Status.ToString()));
                res.Content.WriteBytes(buf);
                buf.Release();
                HttpUtil.SetContentLength(res, res.Content.ReadableBytes);
            }

            // Send the response and close the connection if necessary.
            Task task = ctx.Channel.WriteAndFlushAsync(res);
            if (!HttpUtil.IsKeepAlive(req) || res.Status.Code != 200)
            {
                task.ContinueWith((t, c) => ((IChannelHandlerContext)c).CloseAsync(),
                    ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="e"></param>
        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
        {
            Console.WriteLine($"{nameof(WebSocketServerHandler)} {0}", e);
            ctx.CloseAsync();
        }

        static string GetWebSocketLocation(IFullHttpRequest req)
        {
            bool result = req.Headers.TryGet(HttpHeaderNames.Host, out ICharSequence value);
            //Debug.Assert(result, "Host header does not exist.");
            string location = value.ToString() + WebsocketPath;

            //if (ServerSettings.IsSsl)
            //{
            //    return "wss://" + location;
            //}
            //else
            //{
            return "ws://" + location;
            //}
        }
    }
}
