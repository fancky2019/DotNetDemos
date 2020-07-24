using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    class NewtonsoftHelper
    {
         void Test()
        {
       
            //var jsonStr = JsonConvert.SerializeObject(person1);
            //var jsonStr1 = JsonSerializeObjectFormat(person1);

            //var person = JsonConvert.DeserializeObject<Person>(jsonStr);
            //var dynamicObj = JsonConvert.DeserializeObject<dynamic>(jsonStr);
        }

   

        public static string JsonSerializeObjectFormat(Object obj)
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
