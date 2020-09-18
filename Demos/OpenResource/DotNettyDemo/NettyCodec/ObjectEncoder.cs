using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Protobuf
{
    public class ObjectEncoder : MessageToByteEncoder<object>
    {
        protected override void Encode(IChannelHandlerContext context, object message, IByteBuffer output)
        {
            //发送二进制数据
            //Person data = new Person
            //{
            //    Name = "rui",
            //    Age = 6
            //};
            //model需要使用特性时候注释，如果model加了特性会用model的特性
            MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;

            // Now serializable...
            //15byte
            var messageBytes = MessagePackSerializer.Serialize(message);
            //175 byte
            //var daBytes = Serialization.Serialize<Person>(data);
            //var obj= Serialization.Deserialize<Person>(daBytes);


            IByteBuffer byteBuffer = Unpooled.DirectBuffer(messageBytes.Length);
            byteBuffer.WriteBytes(messageBytes);
            context.WriteAndFlushAsync(byteBuffer);

        }
    }
}
