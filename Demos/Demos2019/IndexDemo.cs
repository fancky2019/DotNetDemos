using Demos.Model;
using System;
using System.Collections;
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
        //        current = _collection[curIndex];
        //    }
        //    return true;
        //}

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
            //索引越界
            //var per1 = indexDemo[10];

            PersonCollection people = new PersonCollection
            {
                Source = new List<Person>
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
                }
            };
            var enumerator = people.GetEnumerator();
            IEnumerable iEnumerable = people;

            //Ctrl+F12跳到 IEnumerator IEnumerable.GetEnumerator()
            var ir = iEnumerable.GetEnumerator();

            //foreach 每循环一次都会调用迭代器GetEnumerator()一次，
            //GetEnumerator() 内循环控制变脸i会在记住，每调用一次加+1。
            foreach (var p in people)
            {
                //没循环一次调用一次 public IEnumerator<Person> GetEnumerator()
            }
        }


    }

    class PersonCollection : IEnumerable<Person>
    {
        public List<Person> Source { get; set; }
        public IEnumerator<Person> GetEnumerator()
        {
            for (int i = 0; i < Source.Count; i++)
            {
                yield return Source[i];
            }
        }
        // java 里接口访问修饰符默认：public abstract
        //c# 接口成员的访问修饰符只能是公有的。
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
