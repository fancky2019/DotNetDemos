using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    /// <summary>
    /// 迭代器的返回类型必须是：IEnumerable、 IEnumerable<T>、 IEnumerator或 IEnumerator<T>。
    /// 
    /// 使用foreach迭代的前提是有GetEnumerator()方法
    /// 
    /// 调用迭代器方法并不会执行迭代器内代码，只有ToList或者枚举成员的时候才会执行迭代器内代码
    /// </summary>
    public class YieldReturnDemo

    {
        public void Test()
        {
            Fun();
        }

        List<int> _list = new List<int>();
        private void Fun()
        {
            _list.Add(1);
            _list.Add(2);
            _list.Add(3);
            _list.Add(4);
            _list.Add(4);
            _list.Add(5);
            _list.Add(6);
            _list.Add(7);
            _list.Add(8);
            _list.Add(9);
            _list.Add(10);
            _list.Add(11);

            /*
             * 使用foreach迭代的前提是有GetEnumerator()方法
             */


            //调用迭代器方法并不会执行迭代器内代码，只有ToList或者枚举成员的时候才会执行迭代器内代码
            IEnumerable<int> iEnumerable = GetEnumerable();
            IEnumerator<int> iEnumerator = iEnumerable.GetEnumerator();


            var list = iEnumerable.ToList();

            foreach(var item in iEnumerable)
            {
                Console.WriteLine($"foreach - {item}");
            }
        }


        public IEnumerable<int> GetEnumerable()
        {
            foreach (var item in _list)
            {
                if (item >= 10)
                {
                    //退出当前迭代
                    yield break;
                }
                Console.WriteLine($"GetEnumerable - {item}");
                if (item % 3 == 0)
                {
                    // yield return返回值调用迭代器的foreach
                    yield return item + 2;
                    //调用迭代器的foreach内代码执行完之后继续执行下面语句
                    Console.WriteLine($"Continue - {item} after yield return ");
                }


            }

        }
    }
}
