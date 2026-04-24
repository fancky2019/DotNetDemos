using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2023
{
    public class EnumReflection
    {

        public void Test()
        {
            var list = GetEnumInfos<TestEnum>();
            var jsonStr = JsonConvert.SerializeObject(list);
            // var jsonStr1 = JsonSerializeObjectFormat(person1);

            //  var person = JsonConvert.DeserializeObject<Person>(jsonStr);

        }

        public Dictionary<string, int> GetEnumValueDes<T>() where T : Enum
        {
            var dictionary = new Dictionary<string, int>();
            Type type = typeof(T);
            foreach (var item in Enum.GetValues(type))
            {
                var name = item.ToString(); //英文名 
                var value = (int)item;  // 数字 value 1
                //获取中文描述需要用到MemberInfo/DescriptionAttribute
                MemberInfo[] memInfo = type.GetMember(name);
                object[] descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionAttributes.Length > 0)
                {
                    //中文描述 是
                    var desc = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                    //把数字当成key，把中文描述当成value添加到字典
                    dictionary.Add(desc, value);
                }
            }
            return dictionary;
        }

        public List<EnumInfo> GetEnumInfos<T>() where T : Enum
        {
            List<EnumInfo> list = new List<EnumInfo>();
            var dic = GetEnumValueDes<TestEnum>();
            EnumInfo enumInfo = null;
            foreach (var item in dic)
            {
                enumInfo = new EnumInfo();
                enumInfo.Name = item.Key;
                enumInfo.Value = item.Value;
                list.Add(enumInfo);
            }
            return list;
        }

    }

    public enum TestEnum
    {
        [Description("空")]
        None = 0,
        [Description("校验")]
        Check = 1


    }

    public class EnumInfo
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
