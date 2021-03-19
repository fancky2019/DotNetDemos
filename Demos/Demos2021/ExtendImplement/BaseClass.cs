using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021.ExtendImplement
{
    public abstract class BaseClass : InterfaceA
    {

        /*
         * 指定抽象不提供具体实现
         * 指定public 访问修饰符实现接口
         */
        public abstract string FunA();

        //public virtual string FunA()
        //{
        //    return "BaseClass+FunA";
        //}

        //public  string FunA()
        //{
        //    return "BaseClass+FunA";
        //}

        public string FunB()
        {
            return "BaseClass+funB";
        }
    }
}
