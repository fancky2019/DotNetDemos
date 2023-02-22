using Demos.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Demos.Demos2019
{
    class CollectionDemo
    {
        public void Test()
        {
            // Sort();
            //  DictionaryTest();
            //SetTest();
            Delete();
        }

        private void Sort()
        {
            List<Person> list = new List<Person>()
            {
                new Person
                {
                    Name="li",
                    Age=1
                },
                new Person
                {
                    Name="rui",
                    Age=6
                },
                new Person
                {
                    Name="fancky",
                    Age=5
                },
                new Person
                {
                    Name="lr",
                    Age=3
                }



            };

            Comparison<Person> comparison = (x, y) => x.Age.CompareTo(y.Age);
            list.Sort(comparison);
            list.Sort((Comparison<Person>)((x, y) => x.Age.CompareTo(y.Age)));
            var orderList = list.OrderBy(p => p.Age).ToList();
        }

        private void DictionaryTest()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("li", "rui");
            //如果添加了重复Key报错，java  不报错，添加不进去
           // dic.Add("li", "fancky");
         //  ConcurrentQueue
           ConcurrentDictionary<string, string> concurrentDictionary = new ConcurrentDictionary<string, string>();
          var re=  concurrentDictionary.TryAdd("li", "rui");//true
          var r1=  concurrentDictionary.TryAdd("li", "fancky");//false
        }

        private void SetTest()
        {
            
            HashSet<string> hashSet = new HashSet<string>();
            hashSet.Add("li");
            hashSet.Add("li");//没有添加进去
           // SortedSet
          
        }

        private void Delete()
        {

            List<int> list = new List<int> { 6, 7, 23, 2, 6, 8 };
            for(int i=list.Count-1; i>=0; i--)
            {
                int num = list[i];
                if(num==6)
                {
                    //内部调用RemoveAt
                    //list.Remove(i);
                    list.RemoveAt(i);
                }
            }

        }
    }
}
