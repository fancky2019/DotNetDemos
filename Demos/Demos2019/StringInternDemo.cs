﻿using System;
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
    class StringInternDemo
    {
        public void Test()
        {
            StringIntern();
        }
        void StringIntern()
        {
            //编译时候能认识的变量都会驻留
            //驻留池HashTable： Key:String Value:托管堆中String对象的引用
            //字面值变量(声明时候赋值的变量)的字符串都会驻留
            //String str1 is known at compile time, and is automatically interned.
            string a = "abc";
            string b = "abc";
            var r1 = a == b;
            var r2 = a.Equals(b);
            var r3 = object.ReferenceEquals(a, b);
            string str = new string(new char[] { 'a', 'b', 'c' });//实例化一个新的String实例
            var r4 = a == str;
            var r5 = a.Equals(str);
            var r6 = object.ReferenceEquals(a, str);//false
            var r61 = a == str;
            string s = string.IsInterned("abc");
            string s1 = string.IsInterned("ab1");
            string s2 = string.Intern("ab1");
            string s3 = string.Intern("ab1");
            string s4 = string.Intern("def");
            var r7 = object.ReferenceEquals(a, s);//true
            var r8 = object.ReferenceEquals(s1, s2);
            var r9 = object.ReferenceEquals(s2, s3);

            string s51 = "abcd";
            //new  
            string s5 = string.IsInterned(new string(new char[] { 'a', 'b', 'c', 'd' }));//在驻留池里找abcd
            string s52 = string.IsInterned("abcd");
            var r51 = object.ReferenceEquals(s51, s5);//true
            var r511 = s51 == s5;
            var r52 = object.ReferenceEquals(s51, s52);//true
            var r522 = s51 == s52;
            string s6 = string.IsInterned(string.Concat(a, b));//null
            string s7 = string.IsInterned(a + b);//null

           
            string s9 = new string(a.ToCharArray());//编译时候不能识别s5;
            var r91 = object.ReferenceEquals(a, s9);//false


            string s92 = string.Intern(new string(a.ToCharArray()));//从驻留池里取
            var r92 = object.ReferenceEquals(a, s92);//true

            //   Copy 方法将返回 String 具有与原始字符串的值相同，但表示不同的对象引用的对象。
            //   它不同于一个赋值运算，它将分配给其他对象变量现有字符串引用。  该示例阐释了不同之处。
            string s8 = string.IsInterned(string.Copy(a));//null
            // String str1 is known at compile time, and is automatically interned.
            var r10 = String.IsInterned(str);
            // Constructed string, str2, is not explicitly or automatically interned.
            string str2 = new StringBuilder().Append("wx").Append("yz").ToString();

            var r11 = String.IsInterned(str2);




            //字符串一单创建就不会改变，如果改变就指向改变的值的地址
            string str3 = str;
            str3 = "str3";//str3指向"str3"的地址。
            var rstt = str3 == str;
            Console.ReadLine();
        }
    }
}