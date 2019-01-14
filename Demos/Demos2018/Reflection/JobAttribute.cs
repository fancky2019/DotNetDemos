using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.Reflection
{
    //添加限定，只能作用于类上
    [AttributeUsage(AttributeTargets.Class)]
    class JobAttribute: Attribute
    {
        public String Description { get; set; }
        public JobAttribute(string description="")
        {
            this.Description = description;
        }
    }
}
