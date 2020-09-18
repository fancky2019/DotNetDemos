using Demos.Demos2020;
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
    public  class ObjectDecoder<T> : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            int readableBytes = message.ReadableBytes;
        
            if (readableBytes > 0)
            {

                try
                {
                    //model需要使用特性时候注释，如果model加了特性会用model的特性
                    MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;

                    var bytes = new byte[readableBytes];
                    message.GetBytes(0, bytes);

                    //二进制字符的换打印
                    //string byteStr = BitConverter.ToString(bytes, 0).Replace("-", " ");
                    //var str = Hex.HexString(bytes);
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
