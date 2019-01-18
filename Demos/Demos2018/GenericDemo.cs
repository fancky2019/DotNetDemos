using Demos.Demos2018.Model;
using System;
using System.Reflection;

namespace DDemos.Demos2018
{
    class GenericDemo
    {
        public void Test()
        {
            //C#使用泛型类必须指定“类型参数”,java不需要
            TClassTest<int> classTest = new TClassTest<int>();
            TClassTest<int>.Display();
            TClassTest<int>.Display<int>();
        }
    }
    class TClassTest<T> where T : new()
    {
        public static void Display()
        {

        }
        public static T1 Display<T1>()
        {
            return Activator.CreateInstance<T1>();
        }

      
        public void Test()
        {
            //TClassTest<Product>.Display();
            CreateInstance();
        }
 
        private void CreateInstance()
        {
            //此种要加泛型约束new
            T t = new T();
            T tt = Activator.CreateInstance<T>();
        }

        private void GetTypes()
        {
            Type myType1 = Type.GetType("System.Int32");
            Type t = typeof(int);
            int a = 10;
            var tt = a.GetType();
        }

        private void GetMember()
        {
            Type type = typeof(Product);
            //获取成员
            PropertyInfo[] propertyInfos = type.GetProperties();
            FieldInfo[] fieldInfos = type.GetFields();
            MethodInfo[] methodInfos = type.GetMethods();
            //获取特性
            propertyInfos[0].GetCustomAttributes();
        }
    }
}
