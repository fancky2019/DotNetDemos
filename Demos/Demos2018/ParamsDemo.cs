using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace DemDemos.Demos2018
{
    class ParamsDemo
    {
        void Fun1(params int[] array)
        {
            if (array == null)
            {
                return;
            }
            int sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
           
        }

        public void Test()
        {
            Fun1(null);
            Fun1(new int[] { 1, 2, 4 });
            Fun1(1);
            Fun1(1, 2, 3);
        }

    }
}
