using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class StaticInnerClassDemo
    {
        public void test()
        {
            // StaticInnerClass staticInnerClass = new StaticInnerClass();
        }

        private int age = 1;

        public StaticInnerClassDemo()
        {
            age = 2;
        }

        public static class StaticInnerClass
        {
            public static String Name;

            static StaticInnerClass()
            {

            }
        }

        public class InnerClass
        {
            public String address;

            public InnerClass()
            {

            }
        }


    }

    class InnerClassTest
    {
        public void test()
        {
            //静态类不能实例化，构造函数不能有访问修饰符
            //  StaticInnerClassDemo.StaticInnerClass staticInnerClass = new StaticInnerClassDemo.StaticInnerClass();

            //非静态内部类实例化
            StaticInnerClassDemo.InnerClass innerClass = new StaticInnerClassDemo.InnerClass();
        }
    }
}
