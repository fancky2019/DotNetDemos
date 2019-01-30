using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.HPSocket
{
    class HPSocketDemo
    {
        public void Test()
        {
            //HPSocketCS源码地址：https://gitee.com/int2e/HPSocketCS

            //注意：将HPSocket4C_U.dll放在根目录下，对应X86还是X64
            /*
             * 报错： System.BadImageFormatException:“未能加载文件或程序集“HPSocketCS, Version=5.4.3.0, Culture=neutral, PublicKeyTo 
             * 原因：HPSocketCS生成的平台和运行程序的平台不一致。
             * 
             */
            Task.Run(() =>
            {
                new HPSocketServer.HPSocketServer().Test();
            });
            Thread.Sleep(10);
            Task.Run(() =>
            {
                new HPSocketClient.HPSocketClient().Test();
            });
        }
    }
}
