using Demos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
  public  class ListSortCompare
    {
        public void  Test()
        {
            Fun();
        }
        private void Fun()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person { Age = 1 });
            list.Add(new Person { Age = 10 });
            list.Add(new Person { Age = 0 });
            list.Add(new Person { Age = -1 });
            list.Add(new Person { Age = 9 });
            list.Sort();
        }
    }
}
