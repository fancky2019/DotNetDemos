using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.SynchronizationDemo
{
    public class GenericDemo
    {
        public void Test()
        {

        }
    }

    public class GenericDemo1<T> where T : Parent, ITest, new()
    {
    }
    public class GenericDemo2<T> where T : class, new()
    {
    }
    public class GenericDemo5<T> where T : class, ITest, new()
    {
    }
    //如果是接口，必须指定具体接口，不能用Interface
    public class GenericDemo3<T> where T : ITest
    {
    }
    public class GenericDemo4
    {
        private T Get<T>() where T : class, new()
        {
            //添加泛型约束就可以
            T t1 = new T();
            //创建泛型对象
            Type type = typeof(T);
            type.Assembly.CreateInstance(type.FullName);
            T t = Activator.CreateInstance<T>();
            return t;
        }

        private void Get<T>(T t)
        {

        }
    }
    public class Parent
    {

    }
    public interface ITest
    {
    }

}
