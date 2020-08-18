using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.ProtoBuf
{
    class ProtoBufNet
    {
        public void Test()
        {
            Serializer.PrepareSerializer<ProtoBufNetModel>();
            //Stream stream = new MemoryStream();
            MemoryStream stream = new MemoryStream();

            ProtoBufNetModel protoBufNetModel = new ProtoBufNetModel { Name = "fancky", Age = 10 };
            Serializer.Serialize<ProtoBufNetModel>(stream, protoBufNetModel);
            var bytes = stream.ToArray();

            var d = Serializer.Deserialize<ProtoBufNetModel>(new MemoryStream(bytes));


            MemoryStream streamList = new MemoryStream();
            List<ProtoBufNetModel> list = new List<ProtoBufNetModel>() { protoBufNetModel };
            Serializer.Serialize(streamList, list);
            var bytesList = streamList.ToArray();
            var dd = Serializer.Deserialize<List<ProtoBufNetModel>>(new MemoryStream(bytesList));

            //反序列化不出来
            //var dds = Serializer.Deserialize<List<ProtoBufNetModel>>(streamList);

        }


    }

    //必须加特性，不然无法序列化。
    [ProtoContract]
    class ProtoBufNetModel
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int Age { get; set; }
    }
}
