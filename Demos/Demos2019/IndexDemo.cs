using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class IndexDemo
    {
        Person[] items = null;

        public Person this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Length)
                {
                    throw new ArgumentOutOfRangeException("索引越界");
                }
                return items[index];
            }
            set
            {
                if (index < 0 || index >= items.Length)
                {
                    throw new ArgumentOutOfRangeException("索引越界");
                }
                items[index] = value;
            }
        }
        public void Test()
        {
            items = new Person[]
            {
                new Person
                {
                    Age=1,
                    Name="fa1"
                },
                  new Person
                {
                    Age=2,
                    Name="fa2"
                },
                    new Person
                {
                    Age=3,
                    Name="fa3"
                }
            };

            var person = items[0];

            IndexDemo indexDemo = new IndexDemo();
            indexDemo.items = items;
            var per = indexDemo[1];
            var per1 = indexDemo[10];
        }


    }
}
