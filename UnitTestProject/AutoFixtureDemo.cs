using AutoFixture.NUnit3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    /// <summary>
    /// 没执行成功，以后待调试
    /// </summary>
    [TestFixture]
    public class AutoFixtureDemo
    {
        [Test]
        [AutoData]
        public void FixValueTest()
        {
            int a = 0, b = 1;
            var result = Add(a, b);
            Assert.AreEqual(a + b, result);
        }
        int Add(int x, int y)
        {
            return x + y;
        }

        [Test, AutoData]
        public void FixValueTest1(int a, int b)
        {

            var result = Add(a, b);
            Assert.AreEqual(a + b, result);
        }
    }
}
