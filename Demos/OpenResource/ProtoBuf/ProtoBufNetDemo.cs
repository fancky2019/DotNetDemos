using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.ProtoBuf
{
    class ProtoBufNetDemo
    {
        public void Test()
        {
            Serializer.PrepareSerializer<ProtoBufNetModelPerson>();
            //Stream stream = new MemoryStream();
            MemoryStream stream = new MemoryStream();

            ProtoBufNetModelPerson protoBufNetModel = new ProtoBufNetModelPerson { Name = "fancky", Age = 10 , 
                Job=new ProtoBufNetModelJob() { Name="chengxuyuan",Salary=9000M}
            };
            Serializer.Serialize<ProtoBufNetModelPerson>(stream, protoBufNetModel);
            var bytes = stream.ToArray();
            //10 byte:fancky-6,10-4。没有多余
            //30 byte
            var d = Serializer.Deserialize<ProtoBufNetModelPerson>(new MemoryStream(bytes));


            MemoryStream streamList = new MemoryStream();
            List<ProtoBufNetModelPerson> list = new List<ProtoBufNetModelPerson>() { protoBufNetModel };
            Serializer.Serialize(streamList, list);
            var bytesList = streamList.ToArray();
            var dd = Serializer.Deserialize<List<ProtoBufNetModelPerson>>(new MemoryStream(bytesList));

            //反序列化不出来
            //var dds = Serializer.Deserialize<List<ProtoBufNetModel>>(streamList);
            var clone = Serializer.DeepClone<ProtoBufNetModelPerson>(protoBufNetModel);

            //AuroMapper

            //没找到支持Json 序列化
            //Newtonsoft.Json;

        }


    }

    //必须加特性，不然无法序列化。
    [ProtoContract]
    class ProtoBufNetModelPerson
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int Age { get; set; }

        //不加特性就不序列化
        [ProtoMember(3)]
        public ProtoBufNetModelJob Job { get; set; }
        
    }
    /*
     * decimal 关键字表示 128 位数据类型。16 byte
     */

    [ProtoContract]
    class ProtoBufNetModelJob
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        //不加特性就不序列化
        //[ProtoMember(2)]
        public decimal Salary { get; set; }
    }

}
