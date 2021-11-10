using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /*
     * 传输控制协议（TCP，Transmission Control Protocol)
     * 
     * 
    * UDP 包的大小就应该是 1500 - IP头(20) - UDP头(8) = 1472(Bytes)
    * TCP 包的大小就应该是 1500 - IP头(20) - TCP头(20) = 1460 (Bytes)
    * MTU:1500,分片，组包
    * UPD:于Internet(非局域网)上的标准MTU值为576字节，最好548字节(576-8-20)以内。
    */
    class TcpClientDemo
    {
        /*
         *    心跳机制：double elapsed = now.Subtract(lastReceivedTime).TotalMilliseconds;
         *              return elapsed >= (2.4 * heartBtIntMillis);
         */
        public void Test()
        {
            Connect("127.0.0.1", "heartBeart");
        }
        void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 7776;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Clint Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Client Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }


    }
}
