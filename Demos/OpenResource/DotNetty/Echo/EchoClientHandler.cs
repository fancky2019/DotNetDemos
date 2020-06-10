using Demos.Common;
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
            /*
             * 如果使用传统的堆内存分配，当我们需要将数据通过socket发送的时候，就需要从堆内存拷贝到直接内存，
             * 然后再由直接内存拷贝到网卡接口层。Netty提供的直接Buffer，直接将数据分配到内存空间，
             * 从而避免了数据的拷贝，实现了零拷贝
             */

            //从堆上分配
            //this.initialMessage = Unpooled.Buffer(256);
            //直接从内存分配
            this.initialMessage = Unpooled.DirectBuffer(1024); 
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
        public override void ChannelActive(IChannelHandlerContext context)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this.initialMessage.WriteBytes(messageBytes);
            context.WriteAndFlushAsync(this.initialMessage);




            //Person data = new Person
            //{
            //    Name = "rui",
            //    Age = 6
            //};

            //MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;

            //// Now serializable...
            ////15byte
            //var messageBytes = MessagePackSerializer.Serialize(data);
            ////175 byte
            ////var daBytes = Serialization.Serialize<Person>(data);
            ////var obj= Serialization.Deserialize<Person>(daBytes);
            //this.initialMessage.WriteBytes(messageBytes);
            //context.WriteAndFlushAsync(this.initialMessage);

        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));

                //这样会造成从堆外的直接内存将数据拷贝到内存堆内，
                //但是可以用Netty的其他特性，比传统Socket仍有优势。
                //var bytes = new byte[byteBuffer.Capacity];
                //byteBuffer.GetBytes(0, bytes);//将数据复制到堆内
                //var contractlessSample = MessagePackSerializer.Deserialize<Person>(bytes);
                //var jsonStr = MessagePackSerializer.ConvertToJson(bytes);
                //Console.WriteLine("Received from server: " + jsonStr);
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
