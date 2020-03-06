using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Demos.Demos2019
{
    public class TCPServerDemo
    {
        private static readonly NLog.Logger _nLog = NLog.LogManager.GetCurrentClassLogger();
        public void Test()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 7776;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[1024];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    while (true)
                    {




                        //// Loop to receive all the data sent by the client.
                        //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        //{
                        //    // Translate data bytes to a ASCII string.
                        //    byte[] lengthBytes = bytes.Take(2).ToArray();

                        //    var msgLength = BitConverter.ToInt16(lengthBytes, 0);
                        //    var data1 = System.Text.Encoding.ASCII.GetString(bytes, 2, 4);
                        //    var data2 = BitConverter.ToUInt64(bytes.Skip(6).Take(8).ToArray(),0);//timestamp
                        //    var data3 = System.Text.Encoding.ASCII.GetString(bytes, 14, i - 14);
                        //    data = $"{data}{data1}{data2}{data3}";
                        //    Console.WriteLine("Server Received: {0}", data);

                        //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        //    // Send back a response.
                        //    stream.Write(msg, 0, msg.Length);
                        //    Console.WriteLine("Server Sent: {0}", data);
                        //}

                        while ((i = stream.Read(bytes, 0, 2)) != 0)
                        {
                            byte[] lengthBytes = bytes.Take(2).Reverse().ToArray();

                            //stream.Read(bytes, 0, 2);
                            var msgLength = BitConverter.ToInt16(lengthBytes, 0);
                            Array.Clear(bytes, 0, 2);
                            stream.Read(bytes, 0, msgLength);
                            var data1 = System.Text.Encoding.ASCII.GetString(bytes, 0, 2);//1C
                            var data2 = bytes.Skip(2).Take(1).ToArray()[0];//5档
                            var data3 = bytes.Skip(3).Take(1).ToArray()[0]; //1档
                            var data4 = BitConverter.ToUInt64(bytes.Skip(4).Take(8).ToArray(), 0);//timestamp
                            var data5 = System.Text.Encoding.ASCII.GetString(bytes, 12, msgLength - 12);//消息

                            data = $"{data1}{data2}{data3}{data4}{data5}";
                            Console.WriteLine("Server Received: {0}", data);

                            _nLog.Info(data);
                        }


                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        //// Send back a response.
                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Server Sent: {0}", data);
                    }
                    // Shutdown and end connection
                    //client.Close();
                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }


    }
}
