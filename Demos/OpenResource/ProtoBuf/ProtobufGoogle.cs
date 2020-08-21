using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.opensource.protobuf.model;

namespace Demos.OpenResource.ProtoBuf
{
    /*
     * NuGet:Google.Protobuf
     * GitHub:https://github.com/protocolbuffers/protobuf
     */
    class ProtobufGoogle
    {
        public void Test()
        {



            Job job = new Job() { Name = "chengxuyuan", Salary = 700 };
            Job job1 = new Job() { Name = "nongmin", Salary = 700 };
            Any any = Any.Pack(job);
            PersonProto personProto = new PersonProto()
            {
                Id = 1,
                Name = "fancky",
                Age = 27,
                Gender = Gender.Man
            };

            personProto.Sons.Add("li");
            personProto.Sons.Add("fa");
            personProto.Any.Add(any);
            personProto.Jobs.Add(job);
            personProto.SonJobs.Add("li", job);
            personProto.SonJobs.Add("fa", job1);

            var bytes = personProto.ToByteArray();

            PersonProto personProto1 = PersonProto.Parser.ParseFrom(bytes);

            //Newtonsoft.Json;
            //var jsonStr1 = personProto1.ToJson<PersonProto>();//用此拓展方法。内部使用的是ServiceStack.Text.JsonSerializer.SerializeToString(personProto1);
            //var personProto2 = jsonStr1.FromJson<PersonProto>();//反序列化集合字典没有序列化出来

            //Newtonsoft.Json 会序列化空值
            //var newtonsoftJson = Newtonsoft.Json.JsonConvert.SerializeObject(personProto1);
            //var newtonsoftJsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonProto>(newtonsoftJson);//反序列化报异常。
        }
    }
}
