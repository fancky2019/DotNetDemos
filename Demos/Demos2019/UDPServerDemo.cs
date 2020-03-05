using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class UDPServerDemo
    {
        private static readonly NLog.Logger _nLog = NLog.LogManager.GetCurrentClassLogger();
        public void Test()
        {
            Receive();
        }
        private void Receive()
        {
            //要绑定本地接收消息的端口号
            UdpClient udpClient = new UdpClient(7777, AddressFamily.InterNetwork);
      
            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            while (true)
            {
  
                string data = string.Empty;
                //接收任意的远程主机端口号的信息
                // Blocks until a message returns on this socket from a remote host.
                Byte[] bytes = udpClient.Receive(ref remoteIpEndPoint);
                data = Encoding.ASCII.GetString(bytes);
                Console.WriteLine("Server Receive: {0}", data);

                _nLog.Info(data);


                //var str = remoteIpEndPoint.Address.ToString();//127.0.0.1
                //remoteIpEndPoint.Port//10000
                //udpClient.Send(bytes,bytes.Length,"127.0.0.1", 10000);


                //udpClient.Send(bytes, bytes.Length, remoteIpEndPoint.Address.ToString(), remoteIpEndPoint.Port);
                udpClient.Send(bytes, bytes.Length, remoteIpEndPoint);


            }
        }
    }
}
