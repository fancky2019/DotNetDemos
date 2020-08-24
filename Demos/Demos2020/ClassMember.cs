using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    /// <summary>
    /// 访问修饰符参见：Demo2019.AccessDecorateDemo
    /// 
    /// 
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

        //抽象方法方法只能声明在抽象类中。

        //抽象、虚拟不能使私有的，子类可以重写。

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


        //索引器

        private int[] arr = new int[100];
        public int this[int index]   // indexer declaration
        {
            get
            {
                // The arr object will throw IndexOutOfRange exception.
                return arr[index];
            }
            set
            {
                arr[index] = value;
            }
        }


        /*子类无法直接调用父类的事件。
         * 解决办法：在父类写一个虚方法，在虚方法内调用事件。子类重写父类虚方法，子类虚方法内调用base.父类虚方法。
         * protected virtual void OnShapeChanged(ShapeEventArgs e)
            {
                // Make a temporary copy of the event to avoid possibility of
                // a race condition if the last subscriber unsubscribes
                // immediately after the null check and before the event is raised.
                EventHandler<ShapeEventArgs> handler = ShapeChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

          protected override void OnShapeChanged(ShapeEventArgs e)
            {
                // Do any circle-specific processing here.

                // Call the base class event invocation method.
                base.OnShapeChanged(e);
            }

         */


    }

    class SwitchClass
    {
        public const string CommandCode1 = "1";
        public const string CommandCode2 = "2";
        public static string CommandCode3 = "3";
    }
}
