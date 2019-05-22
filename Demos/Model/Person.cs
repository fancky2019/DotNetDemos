using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Demos.Model
{
    [Serializable]
   public class Person:IComparable<Person>
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
