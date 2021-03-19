using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021.ExtendImplement
{
    class ExtendImplementDemo
    {
        public void Test()
        {
            InterfaceA iterfaceA = new ChildrenClassA();
            Console.WriteLine(iterfaceA.FunA());
            Console.WriteLine(iterfaceA.FunB());
            InterfaceA iterfaceA1 = new ChildrenClassB();
            Console.WriteLine(iterfaceA1.FunA());
            Console.WriteLine(iterfaceA1.FunB());
        }

    }
}
