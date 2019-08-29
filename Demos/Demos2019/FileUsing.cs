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
            FileIsUsing(fileName);
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


        public static void SaveTxtFile(string fllPath, List<string> content, FileMode fileMode = FileMode.Create)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(fllPath, fileMode, FileAccess.ReadWrite), System.Text.Encoding.UTF8))
            {
                foreach (string str in content)
                {
                    sw.WriteLine(str);
                }
            }
        }
    }
}
