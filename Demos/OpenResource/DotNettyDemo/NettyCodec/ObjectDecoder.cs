using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Google.Protobuf;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Protobuf
{
    public class ObjectDecoder<T> : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            int readableBytes = message.ReadableBytes;
        
            if (readableBytes > 0)
            {

                try
                {
                    //处理接收二进制数据
                    //ArraySegment<byte> ioBuf = message.GetIoBuffer(0, message.ReadableBytes);

                    //但是可以用Netty的其他特性，比传统Socket仍有优势。
                    MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;
                    var bytes = new byte[readableBytes];
                    message.GetBytes(0, bytes);//将数据复制到堆内
                    var t = MessagePackSerializer.Deserialize<T>(bytes);

                    if (t != null)
                    {
                        output.Add(t);
                    }
                }
                catch (Exception innerException)
                {
                    throw new CodecException(innerException);
                }
                //标记依读取位置，防止重复读取，不然下面报错
                // Message = "ObjectDecoder`1.Decode() did not read anything but decoded a message."
                message.SkipBytes(message.ReadableBytes);
            }
        }
    }
}
