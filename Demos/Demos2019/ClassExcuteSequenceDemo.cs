using System;

namespace Demos.Demos2019
{
    /// <summary>
    /// 类执行顺序：静态字段-->静态构造函数-->类成员字段-->父类静态字段-->父类静态构造函数-->
    ///             父类成员字段-->父类非静态构造函数-->类非静态构造函数            
    ///静态构造函数执行：new、访问当前类静态成员（非常量）时候（ 注：子类访问父类静态成员
    ///（此时当前类是父类，不会执行子类的静态函数），子类访问子类的静态成员，当前类是子类，
    ///只执行子类的静态构造函数，不会执行父类的静态构造函数）。
    ///
    /// 
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


            //F.Fun();
            //new N();
            new NN();


            //访问的是父类的静态成员，当前类是父类，只执行父类的静态构造函数
            //int x = N.X;
            //  int x = N.Y;
            //N.FunM();
            //子类访问子类的静态成员，当前类是子类，只执行子类的静态构造函数，不会执行父类的静态构造函数
            //和java的不一样:java会执行父类的
            //N.FunN();
            //访问常量不执行静态构造函数
            // var va = N.NConstVar;

            //var va = N.MConstVar;
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


    class E
    {
        //2--
        public static int X;
        static E()
        {
            //3--
            X = F.Y + 1;
        }
    }

    class F
    {
        //1--先执行静态字段，引用了E.X跳转到执行E类静态成员.执行完E静态成员，继续此赋值语句
        public static int Y = E.X + 1;
        //4-- 
        static F() { }
        public static void Fun()
        {
            Console.WriteLine("X={0},Y={1}", E.X, F.Y);//1,2
        }
    }


    class M
    {
        public const int MConstVar = 10;
        public static int X = 1;
        private int p = 1;
        static M()
        {
            Console.WriteLine($"static M()");
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
        /*
        * 只要调用了类的静态成员，就会跳转执行类的所属的静态静态成员。
        * 
        * 类执行顺序：静态字段-->静态构造函数-->类成员字段-->父类静态字段-->父类静态构造函数-->
        *             父类成员字段-->父类非静态构造函数-->类非静态构造函数
        * 
        */
        public static int Y = 1;
        public const int NConstVar = 1;
        private int q = 1;
        static N()
        {
            Console.WriteLine($"static N()");
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

    /*
     * 只要调用了类的静态成员，就会跳转执行类的所属的静态静态成员。
    */
    class MM
    {
        /*
         * 常量
         * 
         * 常数表达式是在编译时可被完全计算的表达式。 因此，对于引用类型的常数，可能的值只能是 string 和 null。
         * 
         * 调试不会执行常量的代码行。
         */
        public const int MMConstVar = 10;
        public static int X = 1;
        private int p = 1;
        static MM()
        {
            Console.WriteLine($"static MM(),MMConstVar={MMConstVar},X={X}");
        }
        public static int Z = 1;
        public MM()
        {
            Console.WriteLine("public MM()");
        }

        public static void FunMM()
        {
            Console.WriteLine("public static void FunMM()");
        }
    }
    class NN : MM
    {
        public static int Y = 1;
        public const int NConstVar = 1;
        private int q = 1;
        static NN()
        {
            //调用了父类的静态成员，要转取执行父类的静态成员(静态字段--静态构造函数)。
            Console.WriteLine($"static NN(),MMConstVar={MMConstVar},X={X}");
            //执行完父类静态构造函数后继续执行。
        }
        public NN()
        {
            Console.WriteLine("public NN()");
        }
        public static int QN = 1;
        public static void FunNN()
        {
            Console.WriteLine("public static void FunNN()");
        }
    }

}
