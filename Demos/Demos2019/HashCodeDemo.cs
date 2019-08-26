using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /*
     * 如果两个对象的比较结果相等， GetHashCode 为每个对象的方法必须返回相同的值。 但是，如果两个对象不相等，比较 GetHashCode 两个对象的方法不需要返回不同的值。
GetHashCode 对象的方法必须一致地返回相同的哈希代码，只要是确定返回值的对象的对象状态到没有修改 Equals 方法。 请注意这是仅适用于当前执行的应用程序中，
并且如果再次运行该应用程序，可能返回不同的哈希代码。
为了获得最佳性能，哈希函数应生成均匀分布于包括很大程度聚集索引的输入的所有输入。 含意是对对象状态的小修改会导致对哈希表的最佳性能的生成哈希代码的大型进行修改。
哈希函数应计算成本不高。
GetHashCode 方法不应引发异常。


        GetHashCode生成方法见Person实体类

        HashCode 用于散列表存储的如Dictionary、HashSet等set集合。
        快速判读是否存储了改对象：一、根据hashcode 判读该散列地址是否使用，未使用（没有存储）直接添加。
        二、如果散列地址使用了，判断对象是否相等（Equals），如果相等不添加，如果不相等则散列到新的地址，判断是否添加。

如果不重写hashCode，则返回的是对象的内存地址。
要求同一对象必须要有相同的hashCode，如果hashCode不同也不报错。
     */
    public class HashCodeDemo
    {

        public void Test()
        {
            GetHashCodeObject();
        }

        private void GetHashCodeObject()
        {
            Person person = new Person();
            Person person1 = person;
            Console.WriteLine(person.GetHashCode());
            Console.WriteLine(person.GetHashCode());

            Console.WriteLine(person1.GetHashCode());
            Console.WriteLine(person == person1);


            //
            HashSet<Person> hashSet = new HashSet<Person>();
            hashSet.Add(person);
            Thread.Sleep(2000);
            hashSet.Add(person);
        }
    }


}
