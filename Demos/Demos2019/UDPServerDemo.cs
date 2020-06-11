using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /*
     * RTP:实时传输协议。UDP：单个数据丢失不影响应用。
     * 
     * 
     * UDP 包的大小就应该是 1500 - IP头(20) - UDP头(8) = 1472(Bytes)
     * TCP 包的大小就应该是 1500 - IP头(20) - TCP头(20) = 1460 (Bytes)
     * MTC:1500,分片，租包
     * UPD:于Internet(非局域网)上的标准MTU值为576字节，最好548字节 (576-8-20)以内。
     */
    class UDPServerDemo
    {
        private static readonly NLog.Logger _nLog = NLog.LogManager.GetCurrentClassLogger();
        public void Test()
        {
            //Receive();
            //UDPClientUDPMulticast();
            UDPClientUDPBroadcast();
        }

        #region  单播
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
                //data = Encoding.ASCII.GetString(bytes);
                //Console.WriteLine("Server Receive: {0}", data);


                // Translate data bytes to a ASCII string.
                //byte[] lengthBytes = bytes.Take(2).ToArray();

                //var msgLength = BitConverter.ToInt16(lengthBytes, 0);

                //var data1 = System.Text.Encoding.ASCII.GetString(bytes, 2, 4);

                var data1 = System.Text.Encoding.ASCII.GetString(bytes, 0, 1);//1
                var data2 = System.Text.Encoding.ASCII.GetString(bytes, 1, 1);//A


                var data3 = bytes.Skip(2).Take(1).ToArray()[0];//5档
                var data4 = bytes.Skip(3).Take(1).ToArray()[0]; //1档

                var data5 = BitConverter.ToUInt64(bytes.Skip(4).Take(8).ToArray(), 0);//timestamp
                var data6 = System.Text.Encoding.ASCII.GetString(bytes, 12, bytes.Length - 12);
                data = $"{data1}{data2}{data3}{data4}{data5}{data6}";
                Console.WriteLine("Server Received: {0}", data);








                _nLog.Info(data);


                //var str = remoteIpEndPoint.Address.ToString();//127.0.0.1
                //remoteIpEndPoint.Port//10000
                //udpClient.Send(bytes,bytes.Length,"127.0.0.1", 10000);


                //udpClient.Send(bytes, bytes.Length, remoteIpEndPoint.Address.ToString(), remoteIpEndPoint.Port);
                //udpClient.Send(bytes, bytes.Length, remoteIpEndPoint);


            }
        }
        #endregion

        #region  组播
        private void UDPClientUDPMulticast()
        {
            //要绑定本地接收消息的端口号
            UdpClient udpClient = new UdpClient(6001);
            string msg = "UDPServer Multicast message";
            Byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
            //声明组播组IP
            IPAddress multicasIP = IPAddress.Parse("225.0.0.1");
            IPEndPoint multicastIPEndPoint = new IPEndPoint(multicasIP, 6000);

            ////指定要发送的服务地址，不过不连接要在发送的时候指定，采用下面的重载
            //udpClient.Connect("127.0.0.1", 7777);
            ////Sends a message to the host to which you have connected.
            //udpClient.Send(sendBytes, sendBytes.Length);
            //或者
            //直接往多播地址扔数据
            udpClient.Send(sendBytes, sendBytes.Length, multicastIPEndPoint);
        }
        #endregion

        #region  广播
        private void UDPClientUDPBroadcast()
        {
            //要绑定本地接收消息的端口号
            UdpClient udpClient = new UdpClient(6001);
            string msg = "UDPServer Broadcast message";
            Byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
            //声明组播组IP
            IPAddress broadcastIP = IPAddress.Parse("192.168.1.255");
            IPEndPoint broadcastIPEndPoint = new IPEndPoint(broadcastIP, 6000);

            ////指定要发送的服务地址，不过不连接要在发送的时候指定，采用下面的重载
            //udpClient.Connect("127.0.0.1", 7777);
            ////Sends a message to the host to which you have connected.
            //udpClient.Send(sendBytes, sendBytes.Length);
            //或者
            //直接往广播地址扔数据
            udpClient.Send(sendBytes, sendBytes.Length, broadcastIPEndPoint);
        }
        #endregion
    }
}
