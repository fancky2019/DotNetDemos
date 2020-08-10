using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    /// <summary>
    /// 字段：
    /// 属性：属性封装字段
    /// 
    /// </summary>
    class ClassMember
    {
        //const 关键字用于修饰字段或局部变量的声明。 它指定字段或局部变量的值是常数，不能被修改
        //修饰字段或局部变量
        const int ORDER_ID = 4;
        //volatile 关键字可应用于字段
        volatile int m = 0;
        //抽象方法不能在当前类中调用，要用虚方法
        //抽象虚拟不能使私有的

        //事件会广播：多个地方注册，都会收到回调。

        public void Test()
        {
            string commandCode = "";
            switch (commandCode)
            {
                case SwitchClass.CommandCode1:
                    break;
                case SwitchClass.CommandCode2:
                    break;
                //分支应该为常量
                //case SwitchClass.CommandCode3:
                //    break;
                default:
                    break;
            }
        }

    }

    class SwitchClass
    {
        public const string CommandCode1 = "1";
        public const string CommandCode2 = "2";
        public static string CommandCode3 = "3";
    }
}
