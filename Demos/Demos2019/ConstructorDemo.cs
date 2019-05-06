using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class ConstructorDemo
    {
        public void Test()
        {
            ConstructorChild constructorChild = new ConstructorChild("fancky");
            int m = 0;
        }

    }

    class ConstructorParent
    {
        private Int32 age;
        private String name;
        private String parent = "parent";
        private static String staticParent = "staticParent";


        static ConstructorParent()
        {
            Console.WriteLine("staticConstructorParent");
        }

        public ConstructorParent(String name) : this(name, 0)
        {
            //有点像C#的串联构造函数

        }

        public ConstructorParent(String name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }

    class ConstructorChild : ConstructorParent
    {
        private String address;
        private Double salary;
        private String child = "child";
        private static String staticChild = "staticChild";



        static ConstructorChild()
        {
            Console.WriteLine("staticConstructorChild");
        }

        public ConstructorChild(String name) : this(name, 0, "", 0d)
        {

        }

        ConstructorChild(String name, String address) : this(name, 0, address, 0d)
        {

        }

        ConstructorChild(String name, int age, String address, Double salary) : base(name, age)
        {
            this.address = address;
            this.salary = salary;
        }
    }
}
