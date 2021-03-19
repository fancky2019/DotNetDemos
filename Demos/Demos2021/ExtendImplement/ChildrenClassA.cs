using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021.ExtendImplement
{
    public class ChildrenClassA : BaseClass
    {
        /*
         * java里可不用加 @Override，C#必须加override
         * 必须添加override实现父类的FunA方法。不加只是覆盖父类的FunA方法
         */
        override
        public string FunA()
        {
            return "ChildrenClassA+funA";
        }


        public new string FunB()
        {
            return "ChildrenClassA+funA";
        }
    }
}
