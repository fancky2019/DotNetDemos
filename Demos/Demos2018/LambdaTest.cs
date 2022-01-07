using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class LambdaTest
    {
        public void  Test()
        {
            Fun();
        }
        private void LinqExtention()
        {

        }
        private void  Fun()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            //Predicate
            //list.Where()
            int m = 0;
            list.ForEach(p=>
            {
                m++;
            });
            Console.WriteLine(m);
        }
    }
}
