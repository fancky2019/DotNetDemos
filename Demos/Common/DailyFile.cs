using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Common
{
    /// <summary>
    /// 一天生成一个文件
    /// </summary>
    public class DailyFile
    {
        static StreamWriter _sw = null;
        private Timer _timer;
        DateTime _createLogTime;

        public DailyFile(string logName)
        {
            _createLogTime = DateTime.Now;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{logName}_{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.log");

            //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{logName}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.log");

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8);
            _timer = new Timer((o) =>
            {
                if (DateTime.Now.Minute != _createLogTime.Minute)
                {

                    while (true)
                    {
                        if (InterLockedExtention.Acquire())
                        {
                            _createLogTime = DateTime.Now;
                            _sw.Close();

                            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{logName}_{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.log");
                            //filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{logName}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.log");
                            _sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8);
                            InterLockedExtention.Release();
                            break;
                        }
                        else
                        {
                            SpinWait spinWait = default(SpinWait);
                            spinWait.SpinOnce();
                        }
                    }
                }
            }, null, 1000, 1000);
        }


        public void WriteLog(string content)
        {
            while (!InterLockedExtention.Acquire())
            {
                SpinWait spinWait = default;
                spinWait.SpinOnce();
            }

            _sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
            //_sw.Flush();
            InterLockedExtention.Release();
        }

        public void Test()
        {
            Task.Run(() =>
            {
                Random random = new Random();

                while (true)
                {
                    string logStr = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff")} Form1 Test";
                    WriteLog(logStr);
                    Thread.Sleep(random.Next(1, 200));
                }
            });
        }
    }
}
