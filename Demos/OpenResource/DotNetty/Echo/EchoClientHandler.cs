using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNetty.Echo
{

    /*
     * ChannelHandlerAdapter提供网络状态（建立连接、读写、连接断开），其设计没有采用事件向外层抛出，只是提供一堆虚方法，
     * 子类必须重写父类相应方法。
     * 
     * 
     * 
     * 
     * 1、事件顺序Added->Registered->Active, 如果连接成功会执行到Active，未成功会在ConnectToServer中获取到异常，
     *    再进行重连。已经连接到服务器后的断线，通过ExceptionCaught、ChannelInactive、HandlerRemoved获取，
     *    然后重连，重连时进行判断，只执行一次ConnectToServer。
     * 2、加入 new IdleStateHandler后，会触发UserEventTriggered事件，可以该事件中进行心跳检测。
     */
    public class EchoClientHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer initialMessage;
        public event Action DisConnected;

        public EchoClientHandler()
        {
            this.initialMessage = Unpooled.Buffer(256);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
        }

        //channelInactive： 处于非活跃状态，没有连接到远程主机。
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("Disconnected from: " + context.Channel.RemoteAddress);
        }

        //channelUnregistered： 已创建但未注册到一个 EventLoop。
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {

        }

        /// <summary>
        /// 连接建立想服务器发送消息
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(this.initialMessage);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
            }
            //避免死循环，客户服务端不停互相发消息
            //context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        /// <summary>
        /// IdleStateHandler(2, 2, 6) 注册的Handler触发
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent eventState)
            {
         
                if (eventState != null)
                {
                    switch (eventState.State)
                    {
                        case IdleState.ReaderIdle:
                            break;
                        case IdleState.WriterIdle:
                            // 长时间未写入数据
                            // 则发送心跳数据
                            // context.WriteAndFlushAsync();
                           // mp.SendData(ExliveCmd.HEART);

                            break;
                        case IdleState.AllIdle:
                            //6秒既没有读，也没有写，即发生了3次没有读写，可认为网络断开。
                            context.DisconnectAsync().Wait();
                            DisConnected();
                            break;
                    }
                }
            }
            base.UserEventTriggered(context, evt);
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

   

    }
}
