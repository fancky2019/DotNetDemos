using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.UDP
{
    public class NettyUDPClientHnadler : SimpleChannelInboundHandler<DatagramPacket>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            IByteBuffer buf = msg.Copy().Content;
            byte[] req = new byte[buf.ReadableBytes];
            buf.ReadBytes(req);
            String body = Encoding.UTF8.GetString(req);
            Console.WriteLine("Client receive " + body);//打印收到的信息
        }
    }
}
