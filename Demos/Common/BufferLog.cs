using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Common
{
    /// <summary>
    /// D:\fancky\Git\C#\Demos\Demos\Demos2019\NLogDemo.cs
    /// 经测试：
    /// 直接往磁盘写50条/ms。
    /// 加入并发队列200条/ms。然后再异步写log。可以提升性能。
    /// </summary>
    public class BufferLog
    {

        private static ConcurrentQueue<string> _logBuffer;
        /// <summary>
        /// 缓冲区大小。默认500。
        /// 本机测试正常每ms可以写250左右
        /// </summary>
        public static int BufferSize { get; set; }
        /// <summary>
        /// 写日记间隔，单位秒。默认1s。
        /// </summary>
        public static int Interval { get; set; }

        static DateTime _createLogTime;

        //static volatile bool _logChanged;
        //static string FilePath
        //{
        //    get
        //    {

        //        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.txt");
        //    }
        //}
        static volatile StreamWriter _sw = null;

        private static Timer _timer;
        static BufferLog()
        {
            BufferSize = 500;
            Interval = 1;
            _logBuffer = new ConcurrentQueue<string>();
            _createLogTime = DateTime.Now;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.txt");

            //_sw = new StreamWriter(File.Open(FilePath, FileMode.Append, FileAccess.Write), System.Text.Encoding.UTF8);
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8);

            _timer = new Timer((o) =>
              {
                  if(DateTime.Now.Day!= _createLogTime.Day)
                  {
              
                      while (true)
                      {
                          if (InterLockedExtention.Acquire())
                          {
                              _createLogTime = DateTime.Now;
                              //_logChanged = true;
                              _sw.Close();
                     

                              filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}\\{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.txt");
                              _sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8);
                              //_logChanged = false;
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

            Task.Run(() =>
            {
                CountPoll();
            });
            Task.Run(() =>
            {
                IntervalPoll();
            });

        }

        static void IntervalPoll()
        {
            while (true)
            {
                Thread.Sleep(Interval * 1000);
                if (InterLockedExtention.Acquire())
                {
                    WriteLog();
                    InterLockedExtention.Release();
                }
            }
        }

        //static int k = 0;
        /// <summary>
        /// 1毫秒轮询一次，达到BufferSize大小就写日志,一直写到剩余条数小于BufferSize。
        /// 由于1ms内写入可能小于100条，也可能大于100条，不能根据打印出的日志1ms内的条数判断执行
        /// </summary>
        static void CountPoll()
        {
            while (true)
            {
                if (InterLockedExtention.Acquire())
                {

                    while (_logBuffer.Count >= BufferSize)
                    {

                        for (int i = 0; i < BufferSize; i++)
                        {
                            if (_logBuffer.TryDequeue(out string content))
                            {
                                _sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
                            }
                        }
                        _sw.Flush();
                    }

                    InterLockedExtention.Release();
                }
        
                Thread.Sleep(1);
                //如果并发量过大
                //SpinWait spinWait = default(SpinWait);
                //spinWait.SpinOnce();
            }
        }




        static void WriteLog()
        {
            while (!_logBuffer.IsEmpty)
            {
                if (_logBuffer.TryDequeue(out string content))
                {
                    _sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
                }
            }
            _sw.Flush();
        }

        public static void LogAsync(string message)
        {
            _logBuffer.Enqueue(message);
        }

        public static void Test()
        {
            for (int i = 0; i < 100000; i++)
            {
                var str = $"Consumer {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Producer:{i} Commands:{i}";
                //  Log.Info<QueueMiddleWare>(str);
                //   QueueMiddleWareDemo.LogStr.Enqueue(str);
                LogAsync(str);
                //WriteDirectly(str);
            }
        }

        /// <summary>
        /// 每次直接写，flush: 50条/ms
        ///经测试：
        /// 直接往磁盘写50条/ms。
        /// 加入并发队列200条/ms。然后再异步写log。可以提升性能。
        /// </summary>
        /// <param name="content"></param>
        private static void WriteDirectly(string content)
        {
            _sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
        }
    }
}
