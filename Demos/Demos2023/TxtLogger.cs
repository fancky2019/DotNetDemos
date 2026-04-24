using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Wms
{
    public class TxtLogger
    {

        public static void Test()
        {

            string pattern = "^[0-9]{4}-[0-9]{2}-[0-9]{2}.*";
            var re = Regex.IsMatch("2023-03-07 16:12:35.168asassasaas", pattern);
            for (int i = 0; i < 300; i++)
            {
                WriteLog($"fanckyTest{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
        }

        public static void WriteLog(string strContentInfo)
        {
            // 记录最后一次不同的 异常(业务)信息, 如果是相同的信息, 不再打 LOG
            // 原因, 可能产生死循环打 LOG, 导致 LOG 文件过大, 设备无法存储
            string strLogPath = $"D:/ELK/logtest";
            try
            {
                // 拼日志格式
                //string str = string.Format("{0} {1, -15}{2, -32}\r\n",
                //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strFromTo, strContentInfo);
                string str = string.Format("{0}: {1, -32}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strContentInfo);

                // 创建日志路径
                if (!Directory.Exists(strLogPath)) Directory.CreateDirectory(strLogPath);

                // 按日期创建日志文件, 日志路径示例: \Log\2011-06-08.log
                string path = string.Format("{0}/log.log", strLogPath);
                StreamWriter sw = File.AppendText(path);

                sw.Write(str); sw.Close();
            }
            catch
            {
            }
        }
        public static void WriteLog(string strFromTo, string strContentInfo)
        {
            // 记录最后一次不同的 异常(业务)信息, 如果是相同的信息, 不再打 LOG
            // 原因, 可能产生死循环打 LOG, 导致 LOG 文件过大, 设备无法存储
            string strLogPath = "D:" + "/WMSLog";
            try
            {
                // 拼日志格式
                //string str = string.Format("{0} {1, -15}{2, -32}\r\n",
                //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strFromTo, strContentInfo);
                string str = string.Format("{0, -32}\r\n", strContentInfo);

                // 创建日志路径
                if (!Directory.Exists(strLogPath)) Directory.CreateDirectory(strLogPath);

                // 按日期创建日志文件, 日志路径示例: \Log\2011-06-08.log
                StreamWriter sw = File.AppendText(string.Format("{0}/{1}.log",
                    strLogPath, DateTime.Now.ToString("yyyy_MM_dd")));

                sw.Write(str); sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteLogWcs(string strFromTo, string strContentInfo)
        {
            // 记录最后一次不同的 异常(业务)信息, 如果是相同的信息, 不再打 LOG
            // 原因, 可能产生死循环打 LOG, 导致 LOG 文件过大, 设备无法存储
            string strLogPath = "D:" + "/WMSLog";
            try
            {
                // 拼日志格式
                string str = string.Format("{0} {1, -15}{2, -32}\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strFromTo, strContentInfo);

                // 创建日志路径
                if (!Directory.Exists(strLogPath)) Directory.CreateDirectory(strLogPath);

                // 按日期创建日志文件, 日志路径示例: \Log\2011-06-08.log
                StreamWriter sw = File.AppendText(string.Format("{0}/{1}wcs.log",
                    strLogPath, DateTime.Now.ToString("yyyy_MM_dd")));

                sw.Write(str); sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteLogMDS(string strFromTo, string strContentInfo)
        {
            // 记录最后一次不同的 异常(业务)信息, 如果是相同的信息, 不再打 LOG
            // 原因, 可能产生死循环打 LOG, 导致 LOG 文件过大, 设备无法存储
            string strLogPath = "D:" + "/WMSLog";
            try
            {
                // 拼日志格式
                string str = string.Format("{0} {1, -15}{2, -32}\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strFromTo, strContentInfo);

                // 创建日志路径
                if (!Directory.Exists(strLogPath)) Directory.CreateDirectory(strLogPath);

                // 按日期创建日志文件, 日志路径示例: \Log\2011-06-08.log
                StreamWriter sw = File.AppendText(string.Format("{0}/{1}mds.log",
                    strLogPath, DateTime.Now.ToString("yyyy_MM_dd")));

                sw.Write(str); sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteLogAgv(string strFromTo, string strContentInfo)
        {
            // 记录最后一次不同的 异常(业务)信息, 如果是相同的信息, 不再打 LOG
            // 原因, 可能产生死循环打 LOG, 导致 LOG 文件过大, 设备无法存储
            string strLogPath = "D:" + "/WMSLog";
            try
            {
                // 拼日志格式
                string str = string.Format("{0} {1, -15}{2, -32}\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strFromTo, strContentInfo);

                // 创建日志路径
                if (!Directory.Exists(strLogPath)) Directory.CreateDirectory(strLogPath);

                // 按日期创建日志文件, 日志路径示例: \Log\2011-06-08.log
                StreamWriter sw = File.AppendText(string.Format("{0}/{1}agv.log",
                    strLogPath, DateTime.Now.ToString("yyyy_MM_dd")));

                sw.Write(str); sw.Close();
            }
            catch
            {
            }
        }
    }
}
