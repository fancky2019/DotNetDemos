using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Subjects
{
    class SubjectTest
    {
        public void Test()
        {
            //  new B();//x=1,y=0
         
            Console.WriteLine("X={0},Y={1}", A.X, B.Y);//X=2,Y=1
        }
    }
    //class A
    //{
    //    public A()
    //    {
    //        PrintFields();
    //    }
    //    public virtual void PrintFields() { }
    //}
    //class B : A
    //{
    //    int x = 1;
    //    int y;
    //    public B()
    //    {
    //        y = -1;
    //    }
    //    public override void PrintFields()
    //    {
    //        Console.WriteLine("x={0},y={1}", x, y);
    //    }
    //}


    class A
    {
        public static int X;
        static A()
        {
            X = B.Y + 1;
        }
    }
    class B
    {
        public static int Y = A.X + 1;
        static B() { }
      
    }
}
