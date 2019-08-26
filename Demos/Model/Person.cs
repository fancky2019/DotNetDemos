using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Demos.Model
{
    /*
     * VS生成GetHashCode()
     * 一、选中类名。二、点击左侧灯泡"快速操作"，Equals 和GetHashCode 方法
     * 要求同一对象必须要有相同的hashCode，如果hashCode不同也不报错。
     */
    [Serializable]
    public class Person : IComparable<Person>
    {
        [JsonProperty("FnakcyName")]
        public string Name { get; set; }
        public int Age { get; set; }

        public int CompareTo(Person other)
        {
            if (other == null) return 1;


            //特殊值0排在最后
            if (this.Age == 0 && other.Age == 0) return 0;
            if (this.Age == 0) return 1;
            if (other.Age == 0) return -1;



            return this.Age.CompareTo(other.Age);
        }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   Name == person.Name &&
                   Age == person.Age;
        }

        /// <summary>
        ///  HashCode不同， HashSet<Person> 对此调用同一对象add,只添加一次
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -1360180430;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Age.GetHashCode();
            return hashCode;
        }


        /// <summary>
        /// HashCode相同， HashSet<Person> 对此调用同一对象add,添加多次，不能判断重复。
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    //Thread.Sleep(2000);
        //    var hashCode = new Random().Next(1, 100);
        //    return hashCode;
        //}

        //Object





        //public override bool Equals(object obj)
        //{
        //   if(obj==null)
        //    {
        //        return false;
        //    }
        //   if(obj is Person person)
        //    {
        //        return this.Name == person.Name && this.Age == person.Age;
        //    }
        //    return false;
        //}
    }
}
