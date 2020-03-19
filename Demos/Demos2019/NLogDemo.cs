using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// NuGet 安装NLog、NLog.Config
    /// github:https://github.com/NLog/NLog
    ///Tutorial: https://github.com/NLog/NLog下的Section: Getting started
    ///
    /// https://nlog-project.org/config/?tab=layout-renderers
    /// </summary>
    class NLogDemo
    {
        private static readonly NLog.Logger nLog = NLog.LogManager.GetCurrentClassLogger();

        public void Test()
        {
            Fun1();
            //Fun();
            //ThroughputTest();
        }

        private void  Fun1()
        {
            for(int i=0;i<150000;i++)
            {
                nLog.Info($"Debug-{i}");
            }
            Console.WriteLine("completed!");
        }

        /// <summary>
        /// 代码配置。实际采用配置文件的方式
        /// </summary>
        static void InitNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }

        public  void Fun()
        {
            nLog.Debug("Debug1");
            nLog.Info("NLogDemo info ");
            nLog.Info("info2");
            nLog.Warn("Warn3");
            try
            {
                int m = int.Parse("m");
            }
            catch (Exception ex)
            {
                nLog.Error(ex, ex.Message);
                nLog.Error(ex, ex.ToString());
            }
        }

        public void ThroughputTest()
        {
            ConcurrentQueue<string> pool = new ConcurrentQueue<string>();
            for(int i =0;i<5000000;i++)
            {
                string str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
                pool.Enqueue(str);
            }

            while(!pool.IsEmpty)
            {
                string msg;
                pool.TryDequeue(out msg);
                nLog.Info(msg);
            }
        }
    }
}
