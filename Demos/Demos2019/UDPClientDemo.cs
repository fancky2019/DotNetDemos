using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class UDPClientDemo
    {
        public void Test()
        {
            Send();
        }

        private void Send()
        {
            //要绑定本地接收消息的端口号
            UdpClient udpClient = new UdpClient(10000);

            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            //指定要发送的服务地址，不过不连接要在发送的时候指定，采用下面的重载
            udpClient.Connect("127.0.0.1", 7777);
            //Sends a message to the host to which you have connected.
            udpClient.Send(sendBytes, sendBytes.Length);
            //或者
            //udpClient.Send(sendBytes, sendBytes.Length,"127.0.0.1", 7777);

            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Byte[] bytes = udpClient.Receive(ref remoteIpEndPoint);

            Console.WriteLine("Client Receive: {0}", Encoding.ASCII.GetString(bytes));
        }

    }
}
