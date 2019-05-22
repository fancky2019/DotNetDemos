using Demos.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
 
    public static class ObjectClone
    {
        public static void  Test()
        {
            Person person = new Person()
            {
                Age = 1,
                Name = "fancky"
            };
            Person clon1 = person.DepthClone<Person>();
            clon1.Age = 10;
        }
        /// <summary>
        /// 深克隆
        /// </summary>
        /// <param name="obj">原始版本对象</param>
        /// <returns>深克隆后的对象</returns>
        public static T DepthClone<T>(this T obj) where T :class
        {
            T clone = Activator.CreateInstance<T>();
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    clone =(T) formatter.Deserialize(stream);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
            }
            return clone;
        }
    }
}
