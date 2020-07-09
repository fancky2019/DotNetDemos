using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.Reflection
{
    /*
     * 使用：
     * 一、利用构造函数传参
      *[Job]
      *[Job("Job1")]
      *[Job("Job2", "1000", "")]
      * 
      * 二、利用属性赋值
      *[Job(Name ="fancky",Salary ="1000",Description ="ke")]
      *    
      */
    //添加限定，只能作用于类上
    [AttributeUsage(AttributeTargets.Class)]
    class JobAttribute: Attribute
    {

        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; }

        public JobAttribute() : this("", 0, "")
        {

        }
        public JobAttribute(string name = "") : this(name, 0, "")
        {
        }
        public JobAttribute(string name = "", int age = 0, string description = "")
        {
            this.Name = name;
            this.Age = age;
            this.Description = description;
        }
    }
}
