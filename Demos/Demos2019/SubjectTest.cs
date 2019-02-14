using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class SubjectTest
    {
        public void Test()
        {
            new B();//1,0
        }
    }
    class A
    {
        public A()
        {
            PrintFields();
        }
        public virtual void PrintFields() { }
    }
    class B : A
    {
        int x = 1;
        int y;
        public B()
        {
            y = -1;
        }
        public override void PrintFields()
        {
            Console.WriteLine("x={0},y={1}", x, y);
        }
    }
}
