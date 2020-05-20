﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class SocketClientDemo
    {
        public void Test()
        {
            //Send();
            UDPClient();
        }

        #region  TCP

        Socket _clientSocket = null;

        private void Connect()
        {
            int port = 6000;
            string host = "127.0.0.1";//服务器端ip地址

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.Connect(ipe);
        }
        private void Send()
        {

            Connect();
            //send message
            string sendStr = "client message";
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
            _clientSocket.Send(sendBytes);

            //receive message
            string recStr = "";
            byte[] recBytes = new byte[1024];
            int receiveLength = 0;
            while ((receiveLength = _clientSocket.Receive(recBytes, recBytes.Length, 0)) > 0)
            {
                recStr += Encoding.ASCII.GetString(recBytes, 0, receiveLength);
                Console.WriteLine($"Clitnt receives : {recStr}");
            }



            _clientSocket.Close();
        }

        #endregion

        #region  UDP
        private void UDPClient()
        {
            Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpClient.Bind(new IPEndPoint(IPAddress.Any, 6000));

     

            //发送
            //指定接收服务器的IP端口
            EndPoint serverPoint = new IPEndPoint(IPAddress.Parse("192.168.1.105"), 6001);
            string message ="UDPClient message";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.SendTo(data, serverPoint);

            //接收
            Task.Run(() =>
            {
                while (true)
                {
                    //用来保存发送方的ip和端口号
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    //buffer 尽可能大
                    // 如果您使用的是不可靠协议，多余的数据将会丢失。
                    byte[] buffer = new byte[4096];
                    //未收到数据前一直阻塞
                    int length = udpClient.ReceiveFrom(buffer, ref remoteEndPoint);
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    Console.WriteLine($"UDPClient receives:{remoteEndPoint.ToString()} - {message}");

                }
            });

        }
        #endregion
    }
}
