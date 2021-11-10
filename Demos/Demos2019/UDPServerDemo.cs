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
     * 用户数据报协议（UDP，User Datagram Protocol）
     * 
     * 
     * RTP:实时传输协议。UDP：单个数据丢失不影响应用。
     * 
     * 
     * UDP 包的大小就应该是 1500 - IP头(20) - UDP头(8) = 1472(Bytes)
     * TCP 包的大小就应该是 1500 - IP头(20) - TCP头(20) = 1460 (Bytes)
     * MTU:1500,分片，组包
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
        /*
   * 组播是一对多的通信，允许同时向大量的接收方发送数据包。一个主机可以有多个组播进程，这些进程可以加入到同一个组播组也可以加入到不同的组播组，主机会跟踪记录当前哪些进程属于哪个组播组。进程随时可以要求主机主动告诉路由器加入和退出哪个组播组，而且每隔一段时间（大概1分钟）路由器也会向它所在的LAN发送一个查询数据包，要求主机告诉路由器它自己属于哪个组播组。支持组播的路由器负责向所有组内成员发送数据包，但不确保每个成员一定会收到。目的地址是组播ip的数据包会被路由器转发到对应组播组内的主机。
   *组播ip地址是D类地址，可以使用224.0.2.0～238.255.255.255这个范围的ip地址，组播进程根据组播ip进行分组。组播进程监听某端口并通过此端口接收数据，当一个组播数据包被转发到主机上的时候，主机会根据数据包的目的端口将数据包交给监听此目的端口的组播进程。
   *如果主机是多网卡，那么此时就需要注意了，一定要设置用哪个网卡发送和接受数据，因为组播是无法跨网段的，否则会导致数据接收不到。
   *MulticastSocket继承于DatagramSocket，因此可以发送也可以接收数据包。MulticastSocket绑定的端口是接收和发送数据的，如果数据包目的端口和此端口一致，则这个程序就能接收到数据包。setNetworkInterface方法是用来绑定网卡的。joinGroup告诉主机该程序要加入到哪个组播组，leaveGroup则是退出组播组。其他用法和DatagramSocket基本一致。
   * 
   *          
   *          
   *
   * 224.0.0.0～224.0.0.255为预留的组播地址（永久组地址），地址224.0.0.0保留不做分配，其它地址供路由协议使用；
   * 224.0.1.0～224.0.1.255是公用组播地址，可以用于Internet；
   * 224.0.2.0～238.255.255.255为用户可用的组播地址（临时组地址），全网范围内有效；
   * 239.0.0.0～239.255.255.255为本地管理组播地址，仅在特定的本地范围内有效。
   */
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
        /*
 * IPv4地址根据网络号和主机号来分，分为A、B、C三类及特殊地址D、E。全0和全1的都保留不用，如A类中的0.0.0.0和127.255.255.255都不使用。
 * A类：(1.0.0.1-126.255.255.254)（默认子网掩码：255.0.0.0或0xFF000000）第一个字节为网络号，后三个字节为主机号，表示为网络--主机--主机--主机。该类IP地址的最前面为“0”，所以地址的网络号取值于1~126之间。共有16777214个主机地址，一般用于大型网络。
 * B类：(128.1.0.1-191.254.255.254)（默认子网掩码：255.255.0.0或0xFFFF0000）前两个字节为网络号，后两个字节为主机号。该类IP地址的最前面为“10”，所以地址的网络号取值于128~191之间。共有65534个主机地址，一般用于中等规模网络。
 * C类：(192.0.1.1-223.255.254.254)（子网掩码：255.255.255.0或0xFFFFFF00）前三个字节为网络号，最后一个字节为主机号。该类IP地址的最前面为“110”，所以地址的网络号取值于192~223之间。共有254个主机地址，一般用于小型网络。
 * D类：是多播地址。(224.0.0.1-239.255.255.254) 该类IP地址的前面4位为“1110”，所以网络号取值于224~239之间；后面28位为组播地址ID。这是一个专门保留的地址。它并不指向特定的网络，目前这一类地址被用在多点广播（Multicasting）中。多点广播地址用来一次寻址一组计算机，它标识共享同一协议的一组计算机。
 * E类：是保留地址，为将来使用保留。(240.0.0.0---255.255.255.254) 该类IP地址的最前面为“1111”，所以网络号取值于240~255之间。
 * 注意：所有的网络空间计算都必须 -2，这是因为要扣除两个保留地址：
 * 主机号全部为1的地址是子网广播地址，如：192.168.1.255 ；
 * 主机号全部为0的地址是代表该子网的网络地址，如：192.168.1.0 ；
 * 1-254才是给主机使用的。
 * 
 * 
 * 
 * 255.255.255.255
 * 广播地址为受限广播，是不被路由发送，但会被送到相同物理网络段上的所有主机，用于主机配置过程中IP数据包的目的地址。
 */
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
