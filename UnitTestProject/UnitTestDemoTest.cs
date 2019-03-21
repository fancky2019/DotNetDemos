using Demos.Demos2019;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    /// <summary>
    /// 测试类共有
    /// 待测试的类、方法必须是共有的可访问
    /// </summary>
    [TestClass]
    public class UnitTestDemoTest
    {
        /*
         * 右键要测试的方法,选择
         * 1)运行测试：
         * 2)调试测试：可以代码调试带测试的方法。
         * 
         * 
         * 运行结果：
         * 测试资源管理器
         * Add:
         * 通过：绿色√
         * 未通过：红色×
         */
        /// <summary>
        /// 方法共有
        /// </summary>
        [TestMethod]
        public void Add()
        {
            int a = 1;
            int b = 2;
            UnitTestDemo unitTestDemo = new UnitTestDemo();
            //如果测试结果和actual参数相等
            //  Assert.AreEqual(unitTestDemo.Add(a, b), 3);
            Assert.AreEqual(unitTestDemo.Add(a,b), 4);

        }
    }
}
