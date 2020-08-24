using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    /*
     * 接口可以包含方法、属性、事件、索引器，或者这四个成员类型的任意组合。接口成员将自动是公共的。
     * 访问修饰符参见：Demo2019.AccessDecorateDemo
     */
    class InterfaceDemo
    {

    }


    interface IInterfaceTest<T, V>
    {
        event Action<string> CallBack;
        string Name { get; set; }

        int GetAge();

        V this[T index]
        {
            get;
            set;
        }

        //非公有的接口成员必须现实实现
        internal int Add();
    }

    class Cla : IInterfaceTest<int, string>
    {
        List<string> _list = new List<string>();
        public string this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //实现接口事件，加了个public
        public event Action<string> CallBack;

        public int GetAge()
        {
            throw new NotImplementedException();
        }

        int IInterfaceTest<int, string>.Add()
        {
            return 0;
        }
    }
}
