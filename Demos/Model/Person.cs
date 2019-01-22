using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Model
{
   public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
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
