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
    /// 频繁打开IO，不可取。
    /// </summary>

    public class LogAsync
    {

        static BlockingCollection<string[]> _blockingCollection = null;
        static ConcurrentQueue<List<string>> _concurrentQueue = null;

        static LogAsync()
        {
            _blockingCollection = new BlockingCollection<string[]>();
            //_concurrentQueue = new ConcurrentQueue<List<string>>();
            Task.Run(() =>
            {
                Poll();
            });

        }




        static void Poll()
        {
            while (true)
            {
                foreach (var content in _blockingCollection.GetConsumingEnumerable())
                {

                    Log(content[0], content[1], content[2]);
                }


                //if( _concurrentQueue.TryDequeue(out List<string> content))
                // {
                //     Log(content[0], content[1], content[2]);
                // }
                //else
                // {
                //     SpinWait spinWait = default(SpinWait);
                //     spinWait.SpinOnce();

                //     //Thread.Sleep(1);
                // }
            }
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="future">合约名称</param>
        /// <param name="sendMsg">发送到二级行情的字符串</param>
        static void Log(string zdProduct, string zdCode, string sendMsg)
        {
            //格式为：1811
            string replaceZDCodeStr = zdCode.Replace(zdProduct, "");
            //if(replaceZDCodeStr.Length>4)//说明是期权，格式：1811 34
            //{
            //    string[] replaceZDCodeStrArray = replaceZDCodeStr.Split(' ');
            //    string excutePrice = replaceZDCodeStrArray[1];//获取执行价。
            //    if(excutePrice.Contains("."))//如果是小数
            //    {
            //        while (excutePrice.EndsWith("0"))
            //        {
            //            excutePrice = excutePrice.TrimEnd('0');
            //        }
            //        excutePrice = excutePrice.TrimEnd('.');
            //    }
            //    replaceZDCodeStr = replaceZDCodeStrArray[0]+" "+excutePrice;
            //}
            string dic = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
             $@"SendMsg\{zdProduct}\{replaceZDCodeStr}\{DateTime.Now.Year}\{DateTime.Now.Month}");
            //$@"SendMsg\{DateTime.Now.Year}\{DateTime.Now.Month}\{DateTime.Now.Day}\{zdProduct}\{zdCode.Replace(zdProduct,"")}");
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            string fileName = Path.Combine(dic, $"{DateTime.Now.Day}.txt");

            using (StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}  {sendMsg}");
            }


        }

        public static void Log(params string[] message)
        {
            if (!_blockingCollection.IsAddingCompleted)
            {
                _blockingCollection.Add(message);

            }

            //_concurrentQueue.Enqueue(message);
        }

        public static void Close()
        {
            _blockingCollection.CompleteAdding();
        }


    }
}
