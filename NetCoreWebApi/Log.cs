using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Common
{
    /// <summary>
    /// NuGet安装log4net
    /// https://github.com/apache/logging-log4net/tree/master/src
    ///  <!--(高) OFF > FATAL > ERROR > WARN > INFO > DEBUG > ALL (低) -->
    /// </summary>
    public class Log
    {
        static ILoggerRepository _loggerRepository;
        static Log()
        {
            InitLog4Net();
        }
        private static void InitLog4Net()
        {
            //将log4net.config右键属性->复制到输出目录：如果较新则复制，否则无法生成日志文件
            //var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            //XmlConfigurator.ConfigureAndWatch(logCfg);

            //// Nlog
            _loggerRepository = LogManager.CreateRepository(Guid.NewGuid().ToString());
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            //如果配置指定了Repository，那么GetLogger的时候也要指定Repository.Name
            XmlConfigurator.ConfigureAndWatch(_loggerRepository, logCfg);
        }
        public static void Fatal<T>(string message)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(typeof(T));
            logger.Fatal(message);
        }
        public static void Fatal(string message, Type type)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Fatal(message);
        }

        public static void Fatal(string message, Type type, Exception exception)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Fatal(message, exception);
        }



        public static void Error<T>(string message)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(typeof(T));
            logger.Error(message);
        }
        public static void Error(string message, Type type)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Error(message);
        }

        public static void Error(string message, Type type, Exception exception)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Error(message, exception);
        }

        public static void Warn<T>(string message)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(typeof(T));
            logger.Warn(message);
        }
        public static void Warn(string message, Type type)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Warn(message);
        }
        public static void Warn(string message, Type type, Exception exception)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Warn(message, exception);
        }

        public static void Info<T>(string message)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(typeof(T));
            logger.Info(message);
        }
        public static void Info(string message, Type type)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Info(message);
        }
        public static void Info(string message, Type type, Exception exception)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Info(message, exception);
        }


        public static void Debug<T>(string message)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(typeof(T));
            logger.Debug(message);
        }
        public static void Debug(string message, Type type)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Debug(message);
        }
        public static void Debug(string message, Type type, Exception exception)
        {
            //var logger = LogManager.GetLogger(_loggerRepository.Name, typeof(T));
            var logger = LogManager.GetLogger(type);
            logger.Debug(message, exception);
        }
    }
}
