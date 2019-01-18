using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demos.Demos2019.Collections
{
    class CollectionDemo
    {
        public void Test()
        {
            Sort();
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
    }
}
