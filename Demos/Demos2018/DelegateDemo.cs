using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class DelegateDemo
    {
        public void Test()

        {
   

        }

        private void Declaration()
        {
            Action action = DelegateFun;
            Action action1 = delegate
            {

            };
            Action action2 = () =>
              {

              };
        }
        private void DelegateFun()
        {

        }
    }
}
