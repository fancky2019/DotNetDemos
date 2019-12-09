using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demos.Demos2019
{

    /// <summary>
    ///handle下载地址： https://docs.microsoft.com/zh-cn/sysinternals/downloads/handle
    /// </summary>
    public class FileUsing
    {
        public void Test()
        {
            string fileName = @"C:\Users\Administrator\Desktop\test.txt";
            //var re = fileName.Replace(".txt", ".zip");
            UseFile(fileName);

            //再次调用被占用的异常
            //文件“C:\Users\Administrator\Desktop\test.txt”正由另一进程使用，因此该进程无法访问此文件。
            //UseFile(fileName);
            //string fileNameLog = @"C:\Users\Administrator\Desktop\test.log";
            //SaveTxtFile(fileNameLog, new List<string> { "ds" });
            //SaveTxtFile(fileNameLog, new List<string> { "ds2" });
            string fileNameLog2 = @"C:\Users\Administrator\Desktop\test2.log";
            SaveTxtFile2(fileNameLog2, new List<string> { "ds" });
            SaveTxtFile2(fileNameLog2, new List<string> { "ds2" });
            //FileIsUsing(fileName);
        }

        private void UseFile(string fileName)
        {
            //using (StreamReader sr = new StreamReader(new FileStream(fllPath, FileMode.Open)))
            //{
            try
            {
                List<string> content = new List<string>();
                StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open));
                while (!sr.EndOfStream)
                {
                    // string line = sr.ReadLine().Trim();
                    content.Add(sr.ReadLine().Trim());
                }
                //不释放资源，保持被占用。
                //sr.Close();
            }
            catch (Exception ex)
            {
                //TT.Common.Log.Error<TxtFile>(ex.ToString());
            }
            //}
        }

        private void FileIsUsing(string fileName)
        {
            //string fileName = @"c:\aaa.doc";//要检查被那个进程占用的文件

            Process tool = new Process();
            tool.StartInfo.FileName = "handle64.exe";
            //tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.Arguments = fileName + " /accepteula";
            tool.StartInfo.UseShellExecute = false;
            tool.StartInfo.RedirectStandardOutput = true;
            tool.Start();
            tool.WaitForExit();


            //
            // Demos.exe pid: 1604   type: File            48: C: \Users\Administrator\Desktop\test.txt
            string outputTool = tool.StandardOutput.ReadToEnd();

            //pid
            string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
            foreach (Match match in Regex.Matches(outputTool, matchPattern))
            {
                var process = Process.GetProcessById(int.Parse(match.Value));
                //var processName = process.ProcessName;
                //process.Kill();
            }

            var pattern = @"(\S+.exe)\b(?=\s+)\b(?=\s+)";
            var match1 = Regex.Matches(outputTool, pattern);
            var processName = match1[0].Value;

        }

        public static List<string> ReadTxtFile(string fllPath)
        {
            List<string> content = new List<string>();
            if (File.Exists(fllPath))
            {
                using (StreamReader sr = new StreamReader(new FileStream(fllPath, FileMode.Open)))
                {
                    try
                    {
                        while (!sr.EndOfStream)
                        {
                            // string line = sr.ReadLine().Trim();
                            content.Add(sr.ReadLine().Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        //TT.Common.Log.Error<TxtFile>(ex.ToString());
                    }
                }
            }
            return content;
        }


        /////// <summary>
        /////// 如果文件不关闭，在资源管理器中文件被占用
        /////// </summary>
        /////// <param name="filePath"></param>
        /////// <param name="content"></param>
        /////// <param name="fileMode"></param>
        ////public static void SaveTxtFile(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        ////{
        ////    using (StreamWriter sw = new StreamWriter(File.Open(filePath, fileMode, FileAccess.Write), System.Text.Encoding.UTF8))
        ////    {
        ////        foreach (string str in content)
        ////        {
        ////            sw.WriteLine(str);
        ////        }
        ////    }
        ////}

        /// <summary>
        /// 
        /// </summary>
        StreamWriter sw = null;

        public  void SaveTxtFile(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        {
            if (sw == null)
            {
                sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write), System.Text.Encoding.UTF8);
                //sw.AutoFlush = true;
            }
            foreach (string str in content)
             {
                sw.WriteLine(str);
              }
            sw.Flush();



        }

        public void SaveTxtFile2(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        {
            if (sw == null)
            {
                sw = new StreamWriter(filePath,true);
            }
            foreach (string str in content)
            {
                sw.WriteLine(str);
            }
            sw.Flush();
        }
    }
}
