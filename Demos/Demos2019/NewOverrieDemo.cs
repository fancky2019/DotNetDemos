using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class NewOverrieDemo
    {
        public void Test()
        {
            //执行该类的方法
            ParentClass p1 = new ParentClass();
            p1.Fun1();
            p1.Fun2();
            //父类指向子类的引用。如果方法被子类重写了，则执行重写的方法。
            ParentClass p = new ChildrenClassA();
            p.Fun1();
            p.Fun2();
            //执行该类的方法
            ChildrenClassA c = new ChildrenClassA();
            c.Fun1();
            c.Fun2();
        }
    }

    class ParentClass
    {
        public virtual void Fun1()
        {
            Console.WriteLine("ParentClass.Fun1");
        }

        public virtual void Fun2()
        {
            Console.WriteLine("ParentClass.Fun2");
        }
    }

    class ChildrenClassA:ParentClass
    {
        /// <summary>
        /// 覆盖父类方法：不管父类是否指向子类的引用，父类调用父类的，子类调用子类的。
        /// </summary>
        public new void Fun1()
        {
            Console.WriteLine("ChildrenClassA.Fun1");
        }

        /// <summary>
        /// 重写父类方法：当父类指向子类引用时。调用此方法，执行的是子类重写的。
        /// </summary>
        public override void Fun2()
        {
            Console.WriteLine("ChildrenClassA.Fun2");
        }
    }
}
