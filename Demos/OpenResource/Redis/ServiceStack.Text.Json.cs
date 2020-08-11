//using ServiceStack.Text;
using ServiceStack.DataAnnotations;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Redis
{
    class ServiceStackJsonDemo
    {
        public void Test()
        {
            PersonInfo personInfo = new PersonInfo()
            {
                Name = "fancky",
                Age = 27,
                JobInfo = null
            };
            PersonInfo personInfo1 = new PersonInfo()
            {
                Name = "li",
                Age = 28,
                JobInfo = new JobInfo() { JobName = "chengxuyuan", Salary = 1 }
            };

            List<PersonInfo> list = new List<PersonInfo>() { personInfo, personInfo1 };
            // ServiceStack.Text默认不序列化空值到json字符串
            //ServiceStack.Text.JsConfig.IncludeNullValues = true;
            var jsonStr = ServiceStack.Text.JsonSerializer.SerializeToString(list);
            var list1 = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<PersonInfo>>(jsonStr);
            //JsConfig.Reset();

            //Newtonsoft.Json 会序列化空值
            var newtonsoftJson = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            var newtonsoftJsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonInfo>>(newtonsoftJson);

        }
    }

    //使用契约必须都加特性。
    //[DataContract]
    //public class PersonInfo
    //{
    //    [DataMember(Name = "姓名")]
    //    public string Name { get; set; }
    //    [Ignore]
    //    public int Age { get; set; }
    //    [DataMember]
    //    public JobInfo JobInfo { get; set; }
    //}


    public class PersonInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public JobInfo JobInfo { get; set; }
    }

    public class JobInfo
    {
        public string JobName { get; set; }
        public decimal Salary { get; set; }
    }
}
