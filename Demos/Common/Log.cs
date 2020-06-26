using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Common
{

    public class LogManager
    {
        static Dictionary<string, Log> _logs = null;
        static SpinLock _spinLock;
        static LogManager()
        {
            _logs = new Dictionary<string, Log>();
            _spinLock = new SpinLock(false);

        }

        public static void Test()
        {
           //var logName = "c\\dd.txt";
           // if (logName.Contains("\\") )
           // {
               
           // }

           // if (logName.Contains("/") )
           // {

           // }
           // if (logName.Contains("."))
           // {

           // }
           // return;
            Thread test1 = new Thread(() =>
             {
                 WriteLog("test1");
             });
            test1.Name = "test1";
            test1.Start();

            Thread test11 = new Thread(() =>
            {
                Thread.Sleep(5000);
                WriteLog("test1");
            });
            test11.Name = "test11";
            test11.Start();

            Thread test2 = new Thread(() =>
            {
                WriteLog("test2");
            });
            test2.Name = "test2";
            test2.Start();

            Thread test22 = new Thread(() =>
            {
                Thread.Sleep(5000);
                WriteLog("test2");
            });
            test22.Name = "test22";
            test22.Start();

            Thread thread = new Thread(() =>
              {
                  Thread.Sleep(5000);
                  WriteLog("test3");

              });
            thread.Name = "test5000";
            thread.Start();


            //for (int i=4;i<=24;i++)
            //{    
            //    Task.Run(() =>
            //    {
            //       //线程启动要时间，线程还没启动完成，外部循环就已经结束，此时25
            //        Thread.Sleep(i*1000);
            //        WriteLog($"test{i}");
            //    });
            //}

            Task.Run(() =>
            {
                Thread.Sleep(6000);
                WriteLog($"test6");
            });
            Task.Run(() =>
            {
                Thread.Sleep(7000);
                WriteLog($"test7");
            });
            Task.Run(() =>
            {
                Thread.Sleep(8000);
                WriteLog($"test8");
            });
            Task.Run(() =>
            {
                Thread.Sleep(9000);
                WriteLog($"test9");
            });
            Task.Run(() =>
            {
                Thread.Sleep(10000);
                WriteLog($"test10");

            });
            Task.Run(() =>
            {
                Thread.Sleep(11000);
                WriteLog($"test11");
            });
            Task.Run(() =>
            {
                Thread.Sleep(12000);
                WriteLog($"test12");
            });
            Task.Run(() =>
            {
                Thread.Sleep(13000);
                WriteLog($"test13");
            });
            Task.Run(() =>
            {
                Thread.Sleep(14000);
                WriteLog($"test14");
            });
            Task.Run(() =>
            {
                Thread.Sleep(15000);
                WriteLog($"test15");
            });
            Task.Run(() =>
            {
                Thread.Sleep(16000);
                WriteLog($"test16");
            });
            Task.Run(() =>
            {
                Thread.Sleep(17000);
                WriteLog($"test17");
            });
            Task.Run(() =>
            {
                Thread.Sleep(18000);
                WriteLog($"test18");
            });
            Task.Run(() =>
            {
                Thread.Sleep(19000);
                WriteLog($"test19");
            });
            Task.Run(() =>
            {
                Thread.Sleep(20000);
                WriteLog($"test20");
            });
            Task.Run(() =>
            {
                Thread.Sleep(21000);
                WriteLog($"test21");
            });
            Task.Run(() =>
            {
                Thread.Sleep(22000);
                WriteLog($"test22");
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        private static void WriteLog(string logName)
        {
            try
            {


                Random random = new Random();
                Log logger = GetLogger(logName);
                int sleepMillSeconds = 0;
                while (true)
                {
                    string logStr = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff")} Form1 Test Last Time Thread- {Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} Sleep {sleepMillSeconds} ms";
                    logger.WriteLog(logStr);
                    sleepMillSeconds = random.Next(100, 2000);
                    Thread.Sleep(sleepMillSeconds);

                }
            }
            catch(Exception ex)
            {
                string logStr = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff")} - {Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} Exception.";
                Console.WriteLine(logStr);
            }
        }

        /// <summary>
        /// 文件名，不包括路径
        /// </summary>
        /// <param name="logName"></param>
        /// <returns></returns>
        public static Log GetLogger(string logName)
        {
            if (logName.Contains("\\") || logName.Contains("/")|| logName.Contains("."))
            {
                throw new Exception("Invalid logName,logName shold not contain path or extension .");
            }
            bool lockToken = false;
            _spinLock.Enter(ref lockToken);
            Log logger = null;
            if (!_logs.TryGetValue(logName, out logger))
            {
                logger = new Log(logName);
                _logs.Add(logName, logger);
            }

            _spinLock.Exit();
            return logger;
        }


    }
    /// <summary>
    /// 一天生成一个文件
    /// </summary>
    public class Log
    {
        private StreamWriter _sw = null;
        private DateTime _createLogTime;
        private SpinLock _spinLock;
        private string _logName;
        private string _filePath;
        public Log(string logName)
        {
            _spinLock = new SpinLock(false);
            _createLogTime = DateTime.Now;
            this._logName = logName;
            CreateLogFile();
        }

        private void CreateLogFile()
        {
            if (_sw != null)
            {
                _sw.Dispose();
                _sw = null;
            }
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}\\{_logName}.log");
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _sw = new StreamWriter(_filePath, true, System.Text.Encoding.UTF8);
        }


        public void WriteLog(string content)
        {
            bool lockToken = false;
            _spinLock.Enter(ref lockToken);
            if (_sw == null)
            {
                throw new Exception("File is disposed!");
            }
            if (DateTime.Now.Day != _createLogTime.Day)
            {
                CreateLogFile();
            }
            _sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
            _spinLock.Exit();
        }

        public void Dispose()
        {
            bool lockToken = false;
            _spinLock.Enter(ref lockToken);
            _sw.Flush();
            _sw.Dispose();
            _sw = null;
            _spinLock.Exit();
        }

        //public void Test()
        //{
        //    //var reateDate = DateTime.Now.Day;
        //    //FileStream fileStream = _sw.BaseStream as FileStream;
        //    //var fileName = fileStream.Name;
        //    //Dispose();
        //    //return;
        //    //Task.Run(() =>
        //    //{
        //    //    Random random = new Random();

        //    //    while (true)
        //    //    {
        //    //        string logStr = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff")} Form1 Test";
        //    //        WriteLog(logStr);
        //    //        Thread.Sleep(random.Next(1, 2000));
        //    //    }
        //    //});
        //}
    }
}
