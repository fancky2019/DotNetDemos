using HPSocketCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.HPSocket.HPSocketServer
{
    class HPSocketServer
    {
        HPSocketCS.TcpPackServer server = new HPSocketCS.TcpPackServer();

        public HPSocketServer()
        {
            Init();
        }
        public void Test()
        {

            Start();
        }

        private void Init()
        {
            // 设置服务器事件
            server.OnPrepareListen += new TcpServerEvent.OnPrepareListenEventHandler(OnPrepareListen);
            server.OnAccept += new TcpServerEvent.OnAcceptEventHandler(OnAccept);
            server.OnSend += new TcpServerEvent.OnSendEventHandler(OnSend);
            server.OnReceive += new TcpServerEvent.OnReceiveEventHandler(OnReceive);
            server.OnClose += new TcpServerEvent.OnCloseEventHandler(OnClose);
            server.OnShutdown += new TcpServerEvent.OnShutdownEventHandler(OnShutdown);

            // 设置包头标识,与对端设置保证一致性
            server.PackHeaderFlag = 0xff;
            // 设置最大封包大小
            server.MaxPackSize = 0x1000;


        }
        private void Start()
        {

            try
            {
                //localhost报错
                server.IpAddress = "127.0.0.1";
                server.Port = 5555;
                // 启动服务
                if (server.Start())
                {

                    Console.WriteLine(string.Format("Server: $Server Start OK -> ({0}:{1})", server.IpAddress, server.Port));
                }
                else
                {
                    Console.WriteLine(string.Format("Server: $Server Start Error -> {0}({1})", server.ErrorMessage, server.ErrorCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DisconnClient(IntPtr connId)
        {
            try
            {
                // 断开指定客户
                if (server.Disconnect(connId, true))
                {
                    Console.WriteLine(string.Format("Server: $({0}) Disconnect OK", connId));
                }
                else
                {
                    Console.WriteLine(string.Format("Server: Disconnect({0}) Error", connId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Stop()
        {
            server.Stop();
        }

        public void Close()
        {
            if (server != null)
            {
                server.Destroy();
            }
        }

        HandleResult OnPrepareListen(TcpServer sender, IntPtr soListen)
        {
            // 监听事件到达了,一般没什么用吧?

            return HandleResult.Ok;
        }

        HandleResult OnAccept(TcpServer sender, IntPtr connId, IntPtr pClient)
        {
            // 客户进入了


            // 获取客户端ip和端口
            string ip = string.Empty;
            ushort port = 0;
            if (server.GetRemoteAddress(connId, ref ip, ref port))
            {
                Console.WriteLine(string.Format(" Server: client {0},{1}:{2}) connected", connId, ip.ToString(), port));
            }
            else
            {
                Console.WriteLine(string.Format(" Server:> [{0},OnAccept] -> Server_GetClientAddress() Error", connId));
            }


            // 设置附加数据
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ConnId = connId;
            clientInfo.IpAddress = ip;
            clientInfo.Port = port;
            if (server.SetExtra(connId, clientInfo) == false)
            {
                Console.WriteLine(string.Format(" Server:> [{0},OnAccept] -> SetConnectionExtra fail", connId));
            }

            return HandleResult.Ok;
        }

        HandleResult OnSend(TcpServer sender, IntPtr connId, byte[] bytes)
        {
            // 服务器发数据了

            Console.WriteLine(string.Format(" Server:> [{0},OnSend] -> ({1} bytes)", connId, bytes.Length));

            return HandleResult.Ok;
        }

        HandleResult OnReceive(TcpServer sender, IntPtr connId, byte[] bytes)
        {
            // 数据到达了
            try
            {
                string msg = Encoding.Default.GetString(bytes);
                // 获取附加数据
                var clientInfo = server.GetExtra<ClientInfo>(connId);
                if (clientInfo != null)
                {
                    // clientInfo 就是accept里传入的附加数据了

                    Console.WriteLine(string.Format(" Server:> [ {0},OnReceive] ->Client: {1}:{2} ({3} )", clientInfo.ConnId, clientInfo.IpAddress, clientInfo.Port, msg));
                }
                else
                {
                    Console.WriteLine(string.Format(" Server:> [{0},OnReceive] -> ({1} )", connId, msg));
                }

                //应答客户端
                string reply = "Server replys message";
                Byte[] replies = Encoding.Default.GetBytes(reply);
                if (server.Send(connId, replies, replies.Length))
                {
                    return HandleResult.Ok;
                }

                return HandleResult.Error;
            }
            catch (Exception)
            {

                return HandleResult.Ignore;
            }
        }

        HandleResult OnClose(TcpServer sender, IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            if (errorCode == 0)
                Console.WriteLine(string.Format(" Server:> [{0},OnClose]", connId));
            else
                Console.WriteLine(string.Format(" Server:> [{0},OnError] -> OP:{1},CODE:{2}", connId, enOperation, errorCode));
            // return HPSocketSdk.HandleResult.Ok;


            if (server.RemoveExtra(connId) == false)
            {
                Console.WriteLine(string.Format(" Server:> [{0},OnClose] -> SetConnectionExtra({0}, null) fail", connId));
            }

            return HandleResult.Ok;
        }

        HandleResult OnShutdown(TcpServer sender)
        {
            // 服务关闭了


            Console.WriteLine(" Server:> [OnShutdown]");
            return HandleResult.Ok;
        }
    }
}
