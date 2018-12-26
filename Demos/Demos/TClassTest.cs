using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos
{
    class TClassTest<T> where T : new()
    {
        public void Test()
        {
            //TClassTest<Product>.Display();
            CreateInstance();
        }
        static void Display()
        {

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
