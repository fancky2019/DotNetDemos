using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
   public class StaticClassTest
    {
        //静态变量属于类，不属于实例
        private static bool _runned = false;
        public StaticClassTest()
        {
            Console.WriteLine("CurrentThreadID:"+Thread.CurrentThread.ManagedThreadId.ToString()+"," +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if(!_runned)
            {
                NewLogFile();
                _runned = true;
            }
        }
        private object sync_ = new object();
        string messageLogFileName_ = "FIX.4.2-ZDDEV_SD-TT_PRICE.2019-08-01 16:01.messages.current";
        private void NewLogFile()
        {
            System.Threading.Timer timer = new System.Threading.Timer((param) =>
            {
                Console.WriteLine("CurrentThreadID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //string logDateStr = messageLogFileName_.Substring(messageLogFileName_.Length - 27, 10);
                //string nowDateStr = DateTime.Now.ToString("yyyy-MM-dd");

                string logDateMinStr = messageLogFileName_.Substring(messageLogFileName_.Length - 33, 16);

                string todayMinStr = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
                //if (logDateStr != nowDateStr)
                if (logDateMinStr != todayMinStr)
                {
                    //DisposedCheck();

                    lock (sync_)
                    {
                

                        //messageLogFileName_ = messageLogFileName_.Replace(logDateStr, nowDateStr);
                        //eventLogFileName_= eventLogFileName_.Replace(logDateStr, nowDateStr);


                        messageLogFileName_ = messageLogFileName_.Replace(logDateMinStr, todayMinStr);
                        //eventLogFileName_ = eventLogFileName_.Replace(logDateMinStr, todayMinStr);

                        Console.WriteLine(messageLogFileName_);
                        //   messageLog_ = new System.IO.StreamWriter(messageLogFileName_, true);
                        //eventLog_ = new System.IO.StreamWriter(eventLogFileName_, true);
                    }
                }
            }, null, 30 * 1000, 1 * 60 * 1000);
            //}, null, 2 * 60 * 1000, 1 * 60 * 60 * 1000);
        }
    }

    /// <summary>
    /// 静态类不能继承（不能继承类、接口）
    /// </summary>
    public  static class StaticClass1//: StaticClassTestInterface//: NotStaticClass1//: StaticClass2
    {
  
    }
    public static class StaticClass2
    {

    }

    public  class NotStaticClass1
    {

    }

    public class StaticClassTestInterface
    {

    }

}
