using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class TryCatchFinallyReturnDemo
    {
        public void Test()
        {
            //  finally没return: try、catch有return 执行return之后再执行finally块，try、catch内return的值如果
            //                        是基本数据类型不受finally语句影响，如果是引用类型将改变暂存值。
            Console.WriteLine($"Get()={Get()}");//异常：0，正常：1。
            Console.WriteLine($"a={a}");//异常：2，正常：1。

            string str = GetString();
            Console.WriteLine(str);

            Person person = GetPerson();
            Console.WriteLine(person.Name);


        }
        int a = 1;

        /// <summary>
        ///  finally 语句不会改变return 语句中暂存的基本数据类型类型的返回值。
        /// </summary>
        /// <returns></returns>
        int Get()
        {
            int m = -1;
            try
            {
                //int i = int.Parse("a");
                m = 1;
                return m;
            }
            catch (Exception ex)
            {
                m = 0;
                a = 1;
                return m;
            }
            finally  //会在return后执行，之后离开方法体
            {
                a = 2;
                m = 2;
                //finally块内不能有return语句： 控制不能离开finally子句主题。java里可以有return并且覆盖try、catch里的return值
                //return m;
            }
        }

        /// <summary>
        /// finally 语句不会改变return 语句中暂存的基本数据类型类型的返回值。
        /// </summary>
        /// <returns></returns>
        String GetString()
        {
            String str = "";
            try
            {
                str = "try";
                return str;
            }
            catch (Exception ex)
            {
                str = "catch";
                return str;
            }
            finally
            {
                str = "finally";
            }


        }


        /// <summary>
        /// finally 语句会改变return 语句中暂存的引用类型的返回值。
        /// </summary>
        /// <returns></returns>
        Person GetPerson()
        {
            Person person = new Person();
            try
            {
                person.Name = "try";
                return person;
            }
            catch (Exception ex)
            {
                person.Name = "catch";
                return person;
            }
            finally
            {
                person.Name = "finally";
            }


        }
    }
}
