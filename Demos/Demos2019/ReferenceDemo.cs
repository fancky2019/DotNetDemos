using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// MSDN  搜索引用类型
    /// 引用类型的变量不直接包含其数据；它包含的是对其数据的引用。 当通过值传递引用类型的参数时，
    /// 有可能更改引用所指向的数据，如某类成员的值。；也就是说，不能使用相同的引用为新类分配内存
    /// 并使之在块外保持。 若要这样做，应使用 ref 或 out 关键字传递参数。
    /// 
    /// 引用传参：
    /// string:不能改变引用对象，除非ref。
    /// class:不能修改其指向的内存地址,不能重新实例化改变其内存地址，除非用ref。。
    /// array:可以改变其元素指向新的内存地址，但不能改变其元素个数,不能重新实例化改变其内存地址，除非用ref。
    /// list: 可修改内元素、添加新的元素，但不能让其内元素指向新的地址,不能重新实例化改变其内存地址，除非用ref。
    /// </summary>
    public class ReferenceDemo
    {
        Person person = new Person { Age = 30, Name = "lirui" };
        public void Test()
        {
            string str = "123";
            StringReference(str);
            string strRef = "456";
            StringReferenceRef(ref strRef);

            Person person1 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person11 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person111 = new Person
            {
                Age = 10,
                Name = "LR"
            };

            Person person1111 = new Person
            {
                Age = 10,
                Name = "LR"
            };

            Fun1(person1);
            Fun11(person11);
            Fun111(ref person111);
            Fun1111(person1111);



            Person person2 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person22 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person222 = new Person
            {
                Age = 10,
                Name = "LR"
            };

            Person person2222 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person22222 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            List<Person> list2 = new List<Person>()
            { person2 };

            List<Person> list22 = new List<Person>()
            { person22 };

            List<Person> list222 = new List<Person>()
            { person222 };

            List<Person> list2222 = new List<Person>()
            { person2222 };
            List<Person> list22222 = new List<Person>()
            { person22222 };

            //list 可修改内元素即可添加新的元素，单不能让其内元素指向新的地址,不能重新实例化。
            Fun2(list2);
            Fun22(list22);
            Fun222(list222);
            Fun2222( list2222);
            Fun22222(ref list22222);

            Person person3 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person33 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person333 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person3333 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person person33333 = new Person
            {
                Age = 10,
                Name = "LR"
            };
            Person[] array3 = new Person[1] { person3 };
            Person[] array33 = new Person[1] { person33 };
            Person[] array333 = new Person[1] { person333 };
            Person[] array3333 = new Person[1] { person3333 };
            Person[] array33333 = new Person[1] { person33333 };

            //数组可以改变其元素指向新的内存地址，单不能改变其元素个数除非用ref，不能重新实例化。
            Fun3( array3);
            Fun33(array33);
            Fun333(array333);
            Fun3333(array3333);
            Fun33333(ref array33333);
        }

        /// <summary>
        /// 字符串参数不改变原来值
        /// </summary>
        /// <param name="str"></param>
        private void StringReference(string str)
        {
            str = "abc";
        }

        /// <summary>
        /// ref 可改变原始值
        /// </summary>
        /// <param name="str"></param>
        private void StringReferenceRef( ref string str)
        {
            str = "abc";
        }
        /// <summary>
        /// 可修改对象的属性值
        /// </summary>
        /// <param name="person"></param>
        private void Fun1(Person person)
        {
            person.Name = "fancky";
        }

        /// <summary>
        /// 不能修改其指向的内存地址,除非用ref 修饰
        /// </summary>
        /// <param name="person"></param>
        private void Fun11(Person person)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            person = p;
        }

        /// <summary>
        /// 通过ref 可修改其指向的地址
        /// </summary>
        /// <param name="person"></param>

        private void Fun111(ref Person person)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            person = p;
        }

        /// <summary>
        /// 不能修改其指向的内存地址,除非用ref 修饰
        /// </summary>
        /// <param name="person"></param>
        private void Fun1111(Person person)
        {
            person = this.person;
        }

        /// <summary>
        /// 可修改元素属性值
        /// </summary>
        /// <param name="list"></param>
        private void Fun2(List<Person> list)
        {
            list.First().Name = "fancky";
        }

        /// <summary>
        /// 不能改变元素引用地址
        /// </summary>
        /// <param name="list"></param>
        private void Fun22(List<Person> list)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            Person person = list.First();
            person = p;
        }

        /// <summary>
        /// 可改变元素个数，添加新元素
        /// </summary>
        /// <param name="list"></param>
        private void Fun222(List<Person> list)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            //Person person = list.First();
            //person = p;

            ////list.Clear();
            //list.Add(p);

            list.Clear();
            list.Add(p);
        }


        /// <summary>
        /// 不能重新分配地址
        /// </summary>
        /// <param name="list"></param>
        private void Fun2222( List<Person> list)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            //Person person = list.First();
            //person = p;

            list = new List<Person> { p };
        }

        /// <summary>
        /// ref  都可以
        /// </summary>
        /// <param name="list"></param>
        private void Fun22222( ref List<Person> list)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            //Person person = list.First();
            //person = p;

            list.Clear();
            list.Add(p);
        }

        /// <summary>
        /// 可改变数组元素属性值
        /// </summary>
        /// <param name="array"></param>
        private void Fun3( Person[] array)
        {
            Person person = array[0];
            array[0].Name = "fancky";
        }

        /// <summary>
        /// 数组元素可指向新的内存地址
        /// </summary>
        /// <param name="array"></param>
        private void Fun33(Person[] array)
        {
            Person p = new Person()
            {
                Name = "fancky"
            };
            array[0] = p;
        }

        /// <summary>
        /// 添加新的元素
        /// </summary>
        /// <param name="array"></param>
        private void Fun333(Person[] array)
        {
            Array.Clear(array, 0, array.Length);
            Person p = new Person()
            {
                Name = "fancky"
            };
            array[0] = p;
        }

        /// <summary>
        /// 不能重新实例化
        /// </summary>
        /// <param name="array"></param>
        private void Fun3333(Person[] array)
        {
            Array.Clear(array, 0, array.Length);
            Person p = new Person()
            {
                Name = "fancky"
            };
            Person p1 = new Person()
            {
                Name = "fancky11"
            };
            array = new Person[] { p, p1 };

        }

        /// <summary>
        /// ref  都可以
        /// </summary>
        /// <param name="array"></param>
        private void Fun33333(ref Person[] array)
        {
            Array.Clear(array, 0, array.Length);
            Person p = new Person()
            {
                Name = "fancky"
            };
            Person p1 = new Person()
            {
                Name = "fancky11"
            };
            array = new Person[] { p, p1 };

        }
    }
}
