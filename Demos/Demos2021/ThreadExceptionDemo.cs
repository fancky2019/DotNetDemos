using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2021
{

/*
不能跨线程捕获异常
 */
    public class ThreadExceptionDemo
    {
        public void Test()
        {
            Fun1();
        }

        private void Fun1()
        {
            try
            {
                Task.Run(() =>
                {
                    //必须在线程内部进行一场处理，无法抛出到外边的另外一个线程。和java一样
                    //int m = int.Parse("m");
                    try
                    {
                        int m = int.Parse("m");
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Thead 内部:" + ex.Message);
                    }

                });
            }
            catch (Exception ex)
            {
                //外层捕获，没有进入catch
                Console.WriteLine("Thead 外部:" + ex.Message);

            }

        }
    }
}
