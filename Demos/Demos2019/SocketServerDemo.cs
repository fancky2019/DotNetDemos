using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class SocketServerDemo
    {
        public void Test()
        {
            //Listen();
            UDPServer();
        }


        #region  TCP

        private void Listen()
        {
            int port = 6000;
            string host = "127.0.0.1";

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sSocket.Bind(ipe);
            sSocket.Listen(0);
            Console.WriteLine("监听已经打开，请等待");

            while (true)
            {
                //没有新的连接会一直阻塞
                Socket serverSocket = sSocket.Accept();
                Console.WriteLine("连接已经建立");
                string recStr = "";
                byte[] recByte = new byte[1024];
                int receiveLength = serverSocket.Receive(recByte, recByte.Length, 0);
                recStr += Encoding.ASCII.GetString(recByte, 0, receiveLength);

                //send message
                Console.WriteLine("server receives:{0}", recStr);
                string sendStr = "server message";
                byte[] sendByte = Encoding.ASCII.GetBytes(sendStr);
                serverSocket.Send(sendByte, sendByte.Length, 0);
                //serverSocket.Close();
                //sSocket.Close();
            }
        }

        #endregion

        #region  UDP
        private void UDPServer()
        {
            Socket udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpServer.Bind(new IPEndPoint(IPAddress.Any, 6001));//绑定端口号和IP
            Console.WriteLine("UDP服务端已经开启");
            Task.Run(() =>
            {
                //用来保存发送方的ip和端口号
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                //buffer 尽可能大
                // 如果您使用的是不可靠协议，多余的数据将会丢失。
                byte[] buffer = new byte[4096];
                //未收到数据前一直阻塞
                int length = udpServer.ReceiveFrom(buffer, ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine($"UDPServer receives:{remoteEndPoint.ToString()} - {message}"  );


                //回答客户端
                string serverMsg = "UDPServer message";
                udpServer.SendTo(Encoding.UTF8.GetBytes(serverMsg), remoteEndPoint);
            });

        }
        #endregion
    }
}
