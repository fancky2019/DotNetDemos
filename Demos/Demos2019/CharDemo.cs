using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class CharDemo
    {
        public void Test()
        {
            Fun();
        }

        private void Fun()
        {
            char ch1 = '1';
            char ch2 = 'a';
            char ch3 = '李';

            //UTF-16
            int count1 = Encoding.Unicode.GetByteCount(new char[] { ch1 }); //2
            int count2 = Encoding.Unicode.GetByteCount(new char[] { ch2 });//2
            int count3 = Encoding.Unicode.GetByteCount(new char[] { ch3 });//2

            //UTF-8
            int count11 = Encoding.UTF8.GetByteCount(new char[] { ch1 });//1
            int count22 = Encoding.UTF8.GetByteCount(new char[] { ch2 });//1
            int count33 = Encoding.UTF8.GetByteCount(new char[] { ch3 });//3

            //简体中文(GB2312)
            int count111 = Encoding.Default.GetByteCount(new char[] { ch1 });//1
            int count222 = Encoding.Default.GetByteCount(new char[] { ch2 });//1
            int count333 = Encoding.Default.GetByteCount(new char[] { ch3 });//2
        }
    }
}
