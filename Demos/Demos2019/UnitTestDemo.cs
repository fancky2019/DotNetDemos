using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 待测试类必须共有，UnitTestProject项目可以访问到
    /// </summary>
    public class UnitTestDemo
    {
        /// <summary>
        /// 待测试方法必须共有，UnitTestProject项目可以访问到
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
