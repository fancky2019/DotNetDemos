using Demos.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Json
{
    class JsonDemo
    {
        public void Test()
        {
            Person person1 = new Person()
            {
                Name = "fancky",
                Age = 28
            };
            var jsonStr = JsonConvert.SerializeObject(person1);
            var jsonStr1 = JsonSerializeObjectFormat(person1);

            var person = JsonConvert.DeserializeObject<Person>(jsonStr);
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(jsonStr);
        }

        private string JsonStringFormat(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        private string JsonSerializeObjectFormat(Object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }
    }
}
