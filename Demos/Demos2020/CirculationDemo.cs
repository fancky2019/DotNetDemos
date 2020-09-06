using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    class CirculationDemo
    {
        List<string> _list = new List<string>();
        ConcurrentDictionary<string, int> _concurrentDic = new ConcurrentDictionary<string, int>();
        ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();
        public void Test()
        {
            _list.Add("1");
            _list.Add("2");
            _list.Add("3");
            _list.Add("4");

            _concurrentDic.TryAdd("1", 1);
            _concurrentDic.TryAdd("2", 2);
            _concurrentDic.TryAdd("3", 3);
            _concurrentDic.TryAdd("4", 4);

            For();
            Foreach();
        }

        private void For()
        {
            try
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    //内部删除
                    if (_list[i] == "2")
                    {
                        _list.Remove(_list[i]);
                        //删除了一个数据，后面数据前移。
                        //i -= 1;
                        //continue;
                    }
                    Console.WriteLine(_list[i]);

                }
            }
            catch (Exception ex)
            {


            }

        }

        private void Foreach()
        {
            try
            {
                //异常
                //集合已修改；可能无法执行枚举操作。
                foreach (var i in _list)
                {
                    if (i == "2")
                    {
                        _list.Remove(i);
                    }
                }
            }
            catch (Exception ex)
            {


            }

            try
            {
                //可以用foreach内删除数据
                foreach (var keyValue in _concurrentDic)
                {
                    //var key = Orders.Keys.ElementAt<string>(i);
                    if (keyValue.Key == "2")
                    {
                        _concurrentDic.TryRemove(keyValue.Key, out _);
                    }
                }
            }
            catch (Exception ex)
            {


            }


        }
    }
}
