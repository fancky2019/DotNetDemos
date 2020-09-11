using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Concurrency;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.WebSocket
{
    public class WebSocketClientHandler : SimpleChannelInboundHandler<object>
    {
        readonly WebSocketClientHandshaker handshaker;
        //注：DotNetty.Common.Concurrency
        readonly DotNetty.Common.Concurrency.TaskCompletionSource completionSource;

        public Action HandshakeComplete;

        public WebSocketClientHandler(WebSocketClientHandshaker handshaker)
        {
            this.handshaker = handshaker;
            this.completionSource = new TaskCompletionSource();
        }

        /// <summary>
        /// 客户端发起握手完成，服务端还未响应握手
        /// </summary>
        public Task HandshakeCompletion => this.completionSource.Task;

        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            //客户端请求握手
            this.handshaker.HandshakeAsync(ctx.Channel).LinkOutcome(this.completionSource);
        }


        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("WebSocket Client disconnected!");
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            IChannel ch = ctx.Channel;
            //服务端响应握手，握手完成
            if (!this.handshaker.IsHandshakeComplete)
            {
                try
                {
                    this.handshaker.FinishHandshake(ch, (IFullHttpResponse)msg);
                    Console.WriteLine("WebSocket Client connected!");
                    this.completionSource.TryComplete();
                    HandshakeComplete?.Invoke();
                }
                catch (WebSocketHandshakeException e)
                {
                    Console.WriteLine("WebSocket Client failed to connect");
                    this.completionSource.TrySetException(e);
                }

                return;
            }


            if (msg is IFullHttpResponse response)
            {
                throw new InvalidOperationException(
                    $"Unexpected FullHttpResponse (getStatus={response.Status}, content={response.Content.ToString(Encoding.UTF8)})");
            }

            if (msg is TextWebSocketFrame textFrame)
            {
                Console.WriteLine($"WebSocket Client received message: {textFrame.Text()}");
            }
            else if (msg is PongWebSocketFrame)
            {
                Console.WriteLine("WebSocket Client received pong");
            }
            else if (msg is CloseWebSocketFrame)
            {
                Console.WriteLine("WebSocket Client received closing");
                ch.CloseAsync();
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            this.completionSource.TrySetException(exception);
            ctx.CloseAsync();
        }
    }
}
