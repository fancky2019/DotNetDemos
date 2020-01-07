using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /**
     * 将全局变量设置为  ThreadLocal，可达到多线程访问同步的效果。
     * 空间换时间
     */
    public class ThreadLocalDemo
    {
        /// <summary>
        /// 此处new 的是ThreadLocal对象，他内部包装的value 在IsValueCreated未创建
        /// 未Null,要实例化赋值。
        /// 
        ///person.Value:当前线程存储的值。
        ///person.Values:所有线程存储的值。
        /// </summary>
        ThreadLocal<Person> person = new ThreadLocal<Person>();

        ThreadLocal<List<Person>> personList = new ThreadLocal<List<Person>>();
        public void Test()
        {
            MultiThread();
        }

        private void MultiThread()
        {
            Task.Run(() =>
            {
                //UtilityFunction("fancky", 1);
                //UtilityFunction1("fancky1", 11);
                //UtilityFunction1("fancky11", 11);

                FunThreadLocalProperty("ThreadLocalProperty1", 1);
                FunThreadLocalProperty("ThreadLocalProperty11", 11);

                //FunThreadLocalPropertyArray("ThreadLocalPropertyArray3", 0);
                //FunThreadLocalPropertyArray("ThreadLocalPropertyArray33", 1);

                ThreadLocalPropertyDispose();
            });
            Task.Run(() =>
            {
                //UtilityFunction("lr1", 22);
                //UtilityFunction1("fancky1", 22);
                //UtilityFunction1("fancky11", 22);

                FunThreadLocalProperty("ThreadLocalProperty2", 2);
                FunThreadLocalProperty("ThreadLocalProperty22", 22);


                //FunThreadLocalPropertyArray("ThreadLocalPropertyArray32", 0);
                //FunThreadLocalPropertyArray("ThreadLocalPropertyArray332", 1);

                ThreadLocalPropertyDispose();
            });
        }

        private void UtilityFunction(string name, int age)
        {
            Person person = new Person() { Age = age, Name = name };
            this.person.Value = person;
            Thread.Sleep(3000);
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} Person:{ this.person.Value.ToString()}");

        }

        private void UtilityFunction1(string name, int age)
        {
            Person person = new Person() { Age = age, Name = name };
            if (!this.personList.IsValueCreated)
            {
                this.personList.Value = new List<Person>() { person };
            }
            else
            {
                this.personList.Value.Add(person);
            }

            Thread.Sleep(3000);
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} Person:{ this.personList.Value.Last().ToString()}");

        }
        ThreadLocalPropertyModel LocalPropertyModel = new ThreadLocalPropertyModel();

        private void FunThreadLocalProperty(string name, int age)
        {
            Person person = new Person() { Age = age, Name = name };
            if (!this.LocalPropertyModel.people.IsValueCreated)
            {
                this.LocalPropertyModel.people.Value = new List<Person>();
                this.LocalPropertyModel.people.Value.Add(person);
            }
            else
            {
                this.LocalPropertyModel.people.Value.Add(person);
            }
            Thread.Sleep(3000);
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} Person:{ this.LocalPropertyModel.people.Value.Last().ToString()}");

        }

        private void FunThreadLocalPropertyArray(string name, int index)
        {
            if (!this.LocalPropertyModel.orders.IsValueCreated)
            {
                this.LocalPropertyModel.orders.Value = new string[5];
                this.LocalPropertyModel.orders.Value[index] = name;
            }
            else
            {
                this.LocalPropertyModel.orders.Value[index] = name;
            }
            Thread.Sleep(3000);
            Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} orderNumber:{   this.LocalPropertyModel.orders.Value[index]}");

        }

        private void ThreadLocalPropertyDispose()
        {
            //if (this.LocalPropertyModel.orders.IsValueCreated)
            //{
            //    this.LocalPropertyModel.orders.Value[0] = null;
            //    //不要释放，避免资源频繁创建
            //    // this.LocalPropertyModel.orders.Dispose();

            //}

            if (this.LocalPropertyModel.people.IsValueCreated)
            {
                this.LocalPropertyModel.people.Value.Clear();
                //不要释放，避免资源频繁创建
                // this.LocalPropertyModel.orders.Dispose();

            }
        }


    }

    class ThreadLocalPropertyModel
    {
        public int ID { get; set; }
        public ThreadLocal<List<Person>> people;
        public ThreadLocal<string[]> orders;

        public ThreadLocalPropertyModel()
        {

            people = new ThreadLocal<List<Person>>();
            orders = new ThreadLocal<string[]>();
        }
    }
}
