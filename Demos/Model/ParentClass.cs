using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Model
{
    public class ParentClass
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public static implicit operator ParentClass(string name)
        {
            ParentClass parentClass = new ParentClass();
            parentClass.Name = name;
            return parentClass;
        }

        public static explicit operator string(ParentClass parentClass)
        {
            return parentClass.Name;
        }


        public override string ToString()
        {
            return $"{this.Name} is {this.Age}.";
        }
    }
}
