using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.MessagePackDemo
{
    /// <summary>
    /// GitHub:https://github.com/neuecc/MessagePack-CSharp
    /// </summary>
    public class MessagePackDemo
    {
        public void Test()
        {
            ModelAddAttributes();
            Contractless();
            Compression();
        }

        private void ModelAddAttributes()
        {
            var sample3 = new Sample3() { Foo = 10, Bar = 20, IgnoreMember = 30 };
            byte[] bytes = MessagePackSerializer.Serialize<Sample3>(sample3);
            Sample3 mc2 = MessagePackSerializer.Deserialize<Sample3>(bytes);

            // You can dump MessagePack binary blobs to human readable json.
            // Using indexed keys (as opposed to string keys) will serialize to MessagePack arrays,
            // hence property names are not available.
            // [99,"hoge","huga"]
            var json = MessagePackSerializer.ConvertToJson(bytes);
            Console.WriteLine(json);
        }

        private void Contractless()
        {

            var data = new ContractlessSample { MyProperty1 = 99, MyProperty2 = 9999 };

            //var bin = MessagePackSerializer.Serialize( data, ContractlessStandardResolver.Options);

            //// {"MyProperty1":99,"MyProperty2":9999}
            //Console.WriteLine(MessagePackSerializer.SerializeToJson(bin));



            //指定默认ContractlessStandardResolver.Options
            // You can also set ContractlessStandardResolver as the default.
            // (Global state; Not recommended when writing library code)
            MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;

            // Now serializable...
            var bytes1 = MessagePackSerializer.Serialize(data);

            var contractlessSample = MessagePackSerializer.Deserialize<ContractlessSample>(bytes1);

            var jsonStr = MessagePackSerializer.SerializeToJson<ContractlessSample>(contractlessSample);
            var jsonStr1 = MessagePackSerializer.ConvertToJson(bytes1);

            var bytesFromJson = MessagePackSerializer.ConvertFromJson(jsonStr1);
            var obj2 = MessagePackSerializer.Deserialize<ContractlessSample>(bytesFromJson);
        }

        private void Compression()
        {
            var data = new Sample3() { Foo = 10, Bar = 20, IgnoreMember = 30 };


            //LZ4 github:https://github.com/lz4/lz4

            //实体带特性
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
            var bytesLZ4 = MessagePackSerializer.Serialize(data, lz4Options);
            var data1 = MessagePackSerializer.Deserialize<Sample3>(bytesLZ4, lz4Options);

            //实体不带特性
            MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;
            var lz4Options1 = MessagePackSerializer.DefaultOptions.WithCompression(MessagePackCompression.Lz4BlockArray);
            var bytesLZ41 = MessagePackSerializer.Serialize(data, lz4Options);
            var data11 = MessagePackSerializer.Deserialize<Sample3>(bytesLZ4, lz4Options);
        
            int m = 0;
            //ProtoBuf
            //using (var ms = new MemoryStream())
            //{
            //    // serialize empty array.
            //    ProtoBuf.Serializer.Serialize<ContractlessSample>(ms, new ContractlessSample { MyProperty1 = 99, MyProperty2 = 9999 });

            //    ms.Position = 0;
            //    var result = ProtoBuf.Serializer.Deserialize<ContractlessSample>(ms);

            //    Console.WriteLine(result.Array == null); // True, null!
            //}
        }

    }

    /// <summary>
    /// 所有的属性都必须指定Key特性，如果不加Key特性就要设定keyAsPropertyName: true
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    //[MessagePackObject]
    public class Sample3
    {
        //不指定Key,必须设置此[MessagePackObject(keyAsPropertyName: true)]
        // No need for a Key attribute
        //[Key("foo")]
        public int Foo { get; set; }



        [Key("bar")]
        public int Bar { get; set; }

        // If want to ignore a public member, you can use the  IgnoreMember attribute
        [IgnoreMember]
        public int IgnoreMember { get; set; }
    }

    public class ContractlessSample
    {
        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }

}
