using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class IteratorsDemo
    {
        public void Test()
        {

            var iEnumerable = YieldReturn();
            //ForEach:通过枚举器迭代集合的当前Current属性。
            //IEnumerator: IEnumerable 通过GetEnumerator 转换成可迭代的IEnumerator

            // 通过循环来返回集合数据的枚举器
            IEnumerator<int> enumerator = iEnumerable.GetEnumerator();

            //Linq拓展:Enumerable静态类型实现的IEnumerable<int>成员的拓展方法
            var list = iEnumerable.ToList();
            list.GetEnumerator();
            //System.Collections.Generic List 类的方法;ForEach
            list.ForEach((i) => Console.WriteLine(i.ToString()));

            ForEach(iEnumerable);
            ForEachRealCode(iEnumerable);
        }

        /// <summary>
        /// ForEach 
        /// </summary>
        /// <param name="collection"></param>
        private void ForEach(IEnumerable<int> collection)
        {
            //foreach 循环的每次迭代都会调用迭代器方法。
            //迭代器方法运行到 yield return 语句时，会返回一个 expression，
            //并保留当前在代码中的位置。 下次调用迭代器函数时，将从该位置重新开始执行。
            foreach (var p in collection)
            {
                Console.WriteLine(p.ToString());
            }
            // linq  拓展
            var list = collection.ToList();
            //System.Collections.Generic List 类的方法;ForEach
            list.ForEach((i) => Console.WriteLine(i.ToString()));
        }

        /// <summary>
        /// ForEach 实际执行的代码
        /// </summary>
        /// <param name="collection"></param>
        private void ForEachRealCode(IEnumerable<int> collection)
        {
            //ForEach:
            //IEnumerator: IEnumerable 通过GetEnumerator 转换成可迭代的IEnumerator
            IEnumerator<int> enumerator = collection.GetEnumerator();
            //MoveNext()内部实现：通过Index  索引自增（++）进行迭代，获取集合（数组或List）的下一个。有点像数据库的游标，向前只读。
            //MoveNext():实现可参考IndexDemo或者MSDN 的IEnumerator列子,通过MoveNext方法，改变当前元素。参照下面MSDN的MoveNext方法。
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                Console.WriteLine(item.ToString());
            }
        }
        //public bool MoveNext()
        //{
        //    //Avoids going beyond the end of the collection.
        //    if (++curIndex >= _collection.Count)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        // Set current box to next item in collection.
        //        curBox = _collection[curIndex];
        //    }
        //    return true;
        //}


        /// <summary>
        /// 使用迭代器方法枚举数据源
        /// 迭代器方法:关键字yield return
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> YieldReturn()
        {

           // yield return < expression >;
         //   yield break;
            int index = 0;
            while (index++ < 10)
                //yield return 语句可一次返回一个元素。
                yield return index;
        }

        /// <summary>
        ///yield return 优化了迭代器方法(GetEnumerator)。
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetEnumerator()
        {
            //假定list 为集合类的全局变量数据源，通过迭代器方法关键字（yield return）简化GetEnumerator
            //不用去通过MoveNext方法去获取Current
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            //  yield return 优化了迭代器方法。
            int index = 0;
            while (index++ < list.Count)
                yield return list[index];
        }

        IEnumerable<Int32> CountWithTimeLimit(DateTime limit)
        {
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    if (DateTime.Now >= limit)
                    {
                        //迭代器中断退出
                        yield break;
                    }
                    yield return i;
                }
            }
            finally
            {
                Console.WriteLine("停止迭代！"); Console.ReadKey();
            }
        }

    }
}
