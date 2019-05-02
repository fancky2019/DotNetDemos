using System;

namespace Demos.Demos2019
{
    /// <summary>
    /// 类执行顺序：静态字段-->静态构造函数-->类成员字段-->父类静态字段-->父类静态构造函数-->
    ///             父类成员字段-->父类非静态构造函数-->类非静态构造函数            
    ///静态构造函数执行：new、访问当前类静态成员时候（注：子类访问父类静态成员（此时当前类是父类，不会执行子类的静态函数），
    ///                                                  子类访问子类的静态成员，当前类是子类，只执行子类的静态构造函数，不会执行父类的静态构造函数）
    ///总结：父类信息执行在子类非静态构造函数执行前执行。和java类加载顺序不一样。
    /// </summary>
    class ClassExcuteSequenceDemo
    {
        //程序入口  static void Main(string[] args) 参数可有可无
        public void Test()
        {
            ////先执行类内字段初始化，然后执行构造函数（先执行父类的字段，父类的构造函数）
            //B b = new B();
            //b.Print();
            //A a = new A();
            //new N();

            //访问的是父类的静态成员，当前类是父类，只执行父类的静态构造函数
            //int x = N.X;
            //N.FunM();
            //子类访问子类的静态成员，当前类是子类，只执行子类的静态构造函数，不会执行父类的静态构造函数
            N.FunN();
        }
    }

    class A
    {
        int m = 0;
        public A()
        {
            Print();
        }


        //由子类实例化，则访问子类重写的方法。
        public virtual void Print() { }
    }

    class B : A
    {
        int x = 1;
        int y;
        public B()
        {
            y = -1;
        }
        public override void Print()
        {
            Console.WriteLine($"x={x},y={y}");
        }
    }

    abstract class C
    {
        private void Display()
        {

        }

        //抽象方法必须在抽象类内
        //抽象方法必须由子类重写
        public abstract void Display(string str);
    }

    class D : C
    {
        public override void Display(string str)
        {
            throw new NotImplementedException();
        }
    }


    //class E
    //{
    //    public static int X;
    //    static E()
    //    {
    //        X = F.Y + 1;
    //    }
    //}

    //class F
    //{
    //    public static int Y = E.X + 1;
    //    static F() { }
    //    static  void Main()
    //    额
    //        Console.WriteLine("X={0},Y={1}", E.X, F.Y);
    //    }
    //}

    class M
    {
        public static int X = 1;
        private int p = 1;
        static M()
        {
            Console.WriteLine("static M()");
        }
        public static int Z = 1;
        public M()
        {
            Console.WriteLine("public M()");
        }

        public static void FunM()
        {
            Console.WriteLine("public static void FunM()");
        }
    }
    class N : M
    {
        //类执行顺序：静态字段-->静态构造函数-->类成员字段-->父类静态字段-->父类静态构造函数-->
        //            父类成员字段-->父类非静态构造函数-->类非静态构造函数
        public static int Y = 1;
        private int q = 1;
        static N()
        {
            Console.WriteLine("static N()");
        }
        public N()
        {
            Console.WriteLine("public N()");
        }
        public static int QN = 1;
        public static void FunN()
        {
            Console.WriteLine("public static void FunN()");
        }
    }

}
