using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 静态类不能实例化
    /// 非静态内部类：可访问外部类的静态、非静态成员
    /// 静态内部类：只能访问外部类的静态成员
    /// </summary>
    public class InnerClassDemo
    {
        public void Test()
        {
            //和java 里的静态内部类的实例化化方式一样。
            OuterClass.InnerClass innerClass = new OuterClass.InnerClass();

        }
    }

    public class OuterClass
    {
        public static string OuterStaticProperty { get; set; }
        public string OuterProperty { get; set; }

        private string _outerField = "outerField";
        public void OutererClassFun()
        {
            Console.WriteLine("OutererClassFun");
        }

        /// <summary>
        /// 可访问外部类的静态、非静态成员
        /// </summary>
        public class InnerClass
        {
            public static string InnererStaticProperty { get; set; }
            public string InnerPropertu { get; set; }

            private string _innererField = "outerField";

            /// <summary>
            /// 可访问外部类的静态、非静态成员
            /// </summary>

            public void InnerClassFun()
            {
                InnerPropertu = "InnerPropertu";
                Console.WriteLine(_innererField);

                OuterClass outerClass = new OuterClass();
                //可访问外部类的私有成员（还是类内）
                Console.WriteLine(outerClass._outerField);
                outerClass.OuterProperty = "OuterPropertu";

                //访问外部类静态成员
                Console.WriteLine(OuterStaticProperty);

            }
        }

        /// <summary>
        /// 只能访问外部类的静态成员
        /// </summary>
        public static class InnerStaticClass
        {
            public static void InnerStaticClassFun()
            {
                //只可访问外部类静态成员。
                Console.WriteLine(OuterStaticProperty);

                OuterClass outerClass = new OuterClass();
                //可访问外部类的私有成员（还是类内）
                Console.WriteLine(outerClass._outerField);
                outerClass.OuterProperty = "OuterPropertu";


            }
        }

        public interface InnerInterface
        {
            void HelloWorld();
        }

        public class InnerImplementClass : InnerInterface
        {
            public void HelloWorld()
            {
                Console.WriteLine("HelloWorld!");
            }
        }
    }


}
