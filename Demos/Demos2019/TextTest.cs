using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class TextTest
    {
        public void Test()
        {
            WriteText("dssdsdsdsd");
            ReadText("");
        }

        private void WriteText(string sendMsg)
        {
            var dic = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(dic, $"{DateTime.Now.Day}.txt");
            //FileMode.Create
            //using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            //{

            //}
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}  {sendMsg}");
                }
            }
        }
        void SaveContent(string fileName, string content)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (!System.IO.Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"data\{fileName}.csv");
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write), System.Text.Encoding.Default))
            {
                sw.WriteLine(content);
            }
        }

        private List<string> ReadText(string fileFullName)
        {
            string fileName = $"{DateTime.Now.Day}.txt";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            List<string> list = new List<string>();
            //System.Text.Encoding.ASCII
            //using (StreamReader sReader = new StreamReader(File.Open(path, FileMode.Open), System.Text.Encoding.ASCII))
            //{
            //}
            using (StreamReader sReader = new StreamReader(File.Open(path, FileMode.Open), System.Text.Encoding.Default))
            {
                while (!sReader.EndOfStream)
                {
                    string oneLine = sReader.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(oneLine))
                    {
                        list.Add(oneLine);
                    }

                }
            }

            return list;
        }
    }
}
