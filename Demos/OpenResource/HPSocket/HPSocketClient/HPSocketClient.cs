using HPSocketCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.HPSocket.HPSocketClient
{
    class HPSocketClient
    {
        HPSocketCS.TcpPackClient client = new HPSocketCS.TcpPackClient();

        public HPSocketClient()
        {
            Init();
        }
        public void Test()
        {
            Start();
        }
        public void Init()
        {
            // 设置client事件
            client.OnPrepareConnect += new TcpClientEvent.OnPrepareConnectEventHandler(OnPrepareConnect);
            client.OnConnect += new TcpClientEvent.OnConnectEventHandler(OnConnect);
            client.OnSend += new TcpClientEvent.OnSendEventHandler(OnSend);
            client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(OnReceive);
            client.OnClose += new TcpClientEvent.OnCloseEventHandler(OnClose);

            // 设置包头标识,与对端设置保证一致性
            client.PackHeaderFlag = 0xff;
            // 设置最大封包大小
            client.MaxPackSize = 0x1000;
        }
        private void Start()
        {


            try
            {
                String ip = "127.0.0.1";
                ushort port = 5555;
                Console.WriteLine(string.Format("Client: $Client Starting ... -> ({0}:{1})", ip, port));
                bool asyncConn = true;
                if (client.Connect(ip, port, asyncConn))
                {
                    Console.WriteLine(string.Format("Client: $Client Start OK -> ({0}:{1})", ip, port));
                }
                else
                {

                    Console.WriteLine(string.Format("Client: $Client Start Error -> {0}({1})", client.ErrorMessage, client.ErrorCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        HandleResult OnPrepareConnect(TcpClient sender, IntPtr socket)
        {
            return HandleResult.Ok;
        }

        HandleResult OnConnect(TcpClient sender)
        {

            // 已连接 到达一次
            Console.WriteLine("Client: connected success");

            Send("Client Sends Message");
            return HandleResult.Ok;
        }

        HandleResult OnSend(TcpClient sender, byte[] bytes)
        {

            // 客户端发数据了
            Console.WriteLine(string.Format("Client: > [{0},OnSend] -> ({1} bytes)", sender.ConnectionId, bytes.Length));

            return HandleResult.Ok;
        }

        HandleResult OnReceive(TcpClient sender, byte[] bytes)
        {
            // 数据到达了
            string msg = Encoding.Default.GetString(bytes);
            Console.WriteLine(string.Format(" Client:> [{0},OnReceive] -> ({1} )", sender.ConnectionId, msg));

            return HandleResult.Ok;
        }

        HandleResult OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            if (errorCode == 0)
                // 连接关闭了
                Console.WriteLine(string.Format(" Client:> [{0},OnClose]", sender.ConnectionId));
            else
                // 出错了
                Console.WriteLine(string.Format("Client: > [{0},OnError] -> OP:{1},CODE:{2}", sender.ConnectionId, enOperation, errorCode));
            return HandleResult.Ok;
        }


        private void Send(string msg)
        {
            try
            {
                if (msg.Length == 0)
                {
                    return;
                }

                byte[] bytes = Encoding.Default.GetBytes(msg);
                IntPtr connId = client.ConnectionId;

                // 发送
                if (client.Send(bytes, bytes.Length))
                {
                    Console.WriteLine(string.Format("Client: $ ({0}) Send OK --> {1}", connId, msg));
                }
                else
                {
                    Console.WriteLine(string.Format("Client: $ ({0}) Send Fail --> {1} ({2})", connId, msg, bytes.Length));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Client: $ Send Fail -->  msg ({0})", ex.Message));
            }

        }


        private void Stop()
        {

            // 停止服务
            if (client.Stop())
            {
                Console.WriteLine(string.Format("Client:Stopped"));
            }
            else
            {
                Console.WriteLine(string.Format("Client: $Stop Error -> {0}({1})", client.ErrorMessage, client.ErrorCode));
            }
        }

        private void Close()
        {
            if (client != null)
            {
                client.Destroy();
            }
        }

    }
}
