using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    public class TxtFile
    {

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
                            string line = sr.ReadLine().Trim();
                            if (!string.IsNullOrEmpty(line))
                            {
                                content.Add(line);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return content;
        }

        ///// <summary>
        ///// 如果文件不关闭，在资源管理器中文件被占用
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <param name="content"></param>
        ///// <param name="fileMode"></param>
        //public static void SaveTxtFile(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        //{
        //    using (StreamWriter sw = new StreamWriter(File.Open(filePath, fileMode, FileAccess.Write), System.Text.Encoding.UTF8))
        //    {
        //        foreach (string str in content)
        //        {
        //            sw.WriteLine(str);
        //        }
        //    }
        //}

        public static void SaveTxtFile(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        {
            //下面构造FileShare为 FileShare.None，在Windows资管员管理器中打开会报文件被占用的异常
            //StreamWriter sw1 = new StreamWriter(File.Open(filePath, fileMode, FileAccess.Write), System.Text.Encoding.UTF8);

            //文本追加只能以只写的方式,下面的构造FileShare为 FileShare.Read,在Windows资管员管理器中可以打开不能修改。
            // new StreamWriter(filePath,true, System.Text.Encoding.UTF8)
        //    using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.ReadWrite), System.Text.Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8))
            {
                foreach (string str in content)
                {
                    sw.WriteLine(str);
                }
            }
        }



        #region  不关闭文件设计:避免频繁打开文件吞吐量降低。

        /// <summary>
        /// 文件被占用，在资源管理器中无法打开该文件
        /// </summary>
        StreamWriter _sw = null;
        bool _disposed = false;
        private void SaveTxtFile2(string filePath, List<string> content, FileMode fileMode = FileMode.Append)
        {
            if (_sw == null)
            {
                // new StreamWriter(filePath,true, System.Text.Encoding.UTF8)
                _sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.ReadWrite), System.Text.Encoding.UTF8);
               // _sw.AutoFlush = true;//批量写完之后再调用sw.Flush();
            }
            foreach (string str in content)
            {
                _sw.WriteLine(str);
            }
            _sw.Flush();

        }

        private void Dispose()
        {
            if (_sw != null) 
            { 
                _sw.Dispose();
            }
            _sw = null;
            _disposed = true;
        }

        private bool DisposedCheck()
        {
            return _disposed;
        }

        #endregion
    }
}
