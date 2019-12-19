using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 可变的参数
    /// params 参数必须是一维数组
    /// </summary>
    class ParamsDemo
    {
        public void Test()
        {
            Fun();//array不为null,长度为0，
            Fun(1, 2);
            int[] arr = { 3, 4};
            Fun(arr);
            //数组初始化语法不能应用于类型推断的匿名
            //var a = { 1, 2 };
        }

        private void  Fun(params int[] array)
        {
            if(array!=null)
            {
                var length = array.Length;
            }
          
        }

        //private void Fun1(params int array)
        //{

        //}
    }
}
