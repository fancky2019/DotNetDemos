using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    /// <summary>
    ///使用 MessagePack序列化，.NET自带的序列化序列化之后的数组太大
    /// </summary>
    public class Serialization
    {
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


                serializer.Serialize(memoryStream, t);
                return memoryStream.ToArray();


            }

        }

        public static T Deserialize<T>(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


                var obj = serializer.Deserialize(memoryStream);
                return (T)obj;


            }

        }


    }
}
