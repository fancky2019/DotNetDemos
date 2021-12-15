using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021
{
    /*
     * C#访问修饰符对重载没有影响，java继承的重载不能比父类的访问修饰符窄
     */
    internal class OverLoadDemo:OverLoadParent
    {
        //编译通过
        //protected string GetName()
        //{
        //    return "name";
        //}

        //编译通过
        //public string GetName()
        //{
        //    return "name";
        //}

        //编译通过
        private string GetName()
        {
            return "name";
        }
    }

    internal class OverLoadParent
    {
        protected string GetName ()
        {
            return "name";
        }
            
    }
}
