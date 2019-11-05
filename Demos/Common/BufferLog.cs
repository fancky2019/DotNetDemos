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
    public class BufferLog
    {

        private static ConcurrentQueue<string> _logBuffer;
        /// <summary>
        /// 缓冲区大小。默认100。
        /// 本机测试正常每ms可以写150左右
        /// </summary>
        public static int BufferSize { get; set; }
        /// <summary>
        /// 写日记间隔，单位秒。默认1s。
        /// </summary>
        public static int Interval { get; set; }

        static string FilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Year}-{DateTime.Now.Month.ToString("D2")}-{DateTime.Now.Day.ToString("D2")}.txt");
            }
        }

        static BufferLog()
        {
            BufferSize = 100;
            Interval = 1;
            _logBuffer = new ConcurrentQueue<string>();
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
                        //++k;
                        using (StreamWriter sw = new StreamWriter(File.Open(FilePath, FileMode.Append, FileAccess.Write), System.Text.Encoding.UTF8))
                        {
                            for (int i = 0; i < BufferSize; i++)
                            {
                                if (_logBuffer.TryDequeue(out string content))
                                {
                                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
                                }
                            }
                        }
                    }

                    InterLockedExtention.Release();
                }
                Thread.Sleep(1);
            }
        }




        static void WriteLog()
        {
            while (!_logBuffer.IsEmpty)
            {
                using (StreamWriter sw = new StreamWriter(File.Open(FilePath, FileMode.Append, FileAccess.Write), System.Text.Encoding.UTF8))
                {

                    if (_logBuffer.TryDequeue(out string content))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {content}");
                    }
                }
            }
        }

        public static void LogAsync(string message)
        {
            _logBuffer.Enqueue(message);
        }
    }
}
