using Demos.Model;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Echo
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void HandlerAdded(IChannelHandlerContext context)
        {
            base.HandlerAdded(context);
        }

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="context"></param>
        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端 - {context.Channel.RemoteAddress.ToString()} 断开连接。");

            base.HandlerRemoved(context);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);

        }
        public override void ChannelUnregistered(IChannelHandlerContext context) => base.ChannelUnregistered(context);


        public override void ChannelActive(IChannelHandlerContext context) => base.ChannelActive(context);


        public override void ChannelInactive(IChannelHandlerContext context) => base.ChannelInactive(context);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {

                Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));



                //ArraySegment<byte> ioBuf = buffer.GetIoBuffer(0, buffer.Capacity);
                //var array = ioBuf.ToArray();

                // return encoding.GetString(ioBuf.Array, ioBuf.Offset, ioBuf.Count);
                //这样会造成从堆外的直接内存将数据拷贝到内存堆内，
                //但是可以用Netty的其他特性，比传统Socket仍有优势。
                ////MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;
                //var bytes = new byte[buffer.Capacity];
                //buffer.GetBytes(0, bytes);//将数据复制到堆内
                //var contractlessSample = MessagePackSerializer.Deserialize<Person>(bytes);
                //var jsonStr = MessagePackSerializer.ConvertToJson(bytes);
                //Console.WriteLine("Received from client: " + jsonStr);



            }
            context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

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
                            break;
                    }
                }
            }
            context.FireUserEventTriggered(evt);
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
