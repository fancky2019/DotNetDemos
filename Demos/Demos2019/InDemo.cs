using Demos.Demos2018.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 泛型接口和委托中的泛型类型参数。
    ///作为参数修饰符，它允许按引用而不是按值向方法传递参数。
    ///foreach 语句。
    /// LINQ 查询表达式中的 from 子句。
    /// LINQ 查询表达式中的 join 子句。
    /// 
    /// 
    /// 
    /// 
    /// 
    /// in 参数修饰符： 引用类型：不能重新实例化
    ///                 基本类型：不能赋值
    /// </summary>
    public class InDemo
    {
        public void Test()
        {
            Product product = new Product();
            product.ID = 100;
            Fun1(product);
            Console.WriteLine(product.ID);
            Fun2(product);
            Console.WriteLine(product.ID);
            int n=100;
            Fun3(n);
            int m = 100;
            Fun4(m);

            string str = "100";
            //string 类型参数，不改变实参值。
            Fun5(str);
            Console.WriteLine(str);
        }

        private void Fun1(Product product)
        {
            //
            product = new Product();
            product.ID = 1;
        }

        /// <summary>
        /// in 修饰无法重新实例化。
        /// </summary>
        /// <param name="product"></param>
        private void Fun2(in Product product)
        {

            //in 修饰无法重新实例化。
           // product = new Product();
            product.ID = 2;
            product.ProductName = "2";
        }

        private void Fun3( int num)
        {
            num = 3;
        }

        /// <summary>
        ///  in 修饰，num 是只读变量，无法修改
        /// </summary>
        /// <param name="num"></param>
        private void Fun4(in int num)
        {
            //num 是只读变量，无法修改
           // num = 4;
        }

        private void Fun5(string str)
        {
            str = "5";
        }

        /// <summary>
        ///  in 修饰，str 是只读变量，无法修改
        /// </summary>
        /// <param name="num"></param>
        private void Fun6(in string str)
        {
          //  str = "6";
        }
    }
}
