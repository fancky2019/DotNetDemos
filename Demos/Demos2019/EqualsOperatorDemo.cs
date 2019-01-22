using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 对于预定义的值类型，如果操作数的值相等，则相等运算符 (==) 返回 true，否则返回 false。 
    /// 对于 string 以外的引用类型，如果两个操作数引用同一个对象，则 == 返回 true。
    /// 对于 string 类型，== 比较字符串的值。
    /// </summary>
    class EqualsOperatorDemo
    {
        public  void  Test()
        {
            EqualOperator();
            Equal();
        }

        private void EqualOperator()
        {
            int a1 = 10;
            int a2 = 9;
            int a11 = 10;
            bool r1 = a1 == a11;// true
            bool r12 = a1 == a2;//false
            string s1 = "abc";
            string s2 = "d";
            string s11 = "abc";
            bool r2 = s1 == s11;// true
            bool r21 = s1 == s2;//false
            Person person1 = new Person()
            {
                Name = "fancky",
                Age =28
            };
            Person person2 = new Person()
            {
                Name = "fancky",
                Age = 29
            };
            Person person11 = new Person()
            {
                Name = "fancky",
                Age = 28
            };
            Person person3 = person1;
            bool r3 = person1 == person11;// false
            bool r32 = person1 == person2;//false
            bool r33 = person1 == person3;//true
        }

        private void Equal()
        {
            int a1 = 10;
            int a2 = 9;
            int a11 = 10;
            bool r1 = a1.Equals(a11);// true
            bool r12 = a1.Equals(a2);//false
            string s1 = "abc";
            string s2 = "d";
            string s11 = "abc";
            bool r2 = s1.Equals(s11); // true
            bool r21 = s1.Equals(s2);//false
            Person person1 = new Person()
            {
                Name = "fancky",
                Age = 28
            };
            Person person2 = new Person()
            {
                Name = "fancky",
                Age = 29
            };
            Person person11 = new Person()
            {
                Name = "fancky",
                Age = 28
            };
            Person person3 = person1;
            bool r3 = person1.Equals(person11);// true,Person重写了Equals，如果不重写false
            bool r32 = person1.Equals(person2);//false
            bool r33 = person1.Equals( person3);//true
         
        }
    }
}
