using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformFormUpdate
{
    public partial class FrmSocketClient : Form
    {
        public FrmSocketClient()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        Socket _clientSocket = null;
        private void Connect()
        {
            int port = int.Parse(this.txtPort.Text);
            string host = this.txtIP.Text;//服务器端ip地址

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.Connect(ipe);

            Task.Run(() =>
            {
                string recStr = "";
                byte[] recBytes = new byte[1024];
                int receiveLength = 0;
                while ((receiveLength = _clientSocket.Receive(recBytes, recBytes.Length, 0)) > 0)
                {
                    recStr += Encoding.ASCII.GetString(recBytes, 0, receiveLength);
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        this.rtbReceiveMsg.Text = $"Clitnt receives : {recStr}";
                    }));
                }

            });
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _clientSocket?.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string sendStr = this.rtbMsg.Text;
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
            _clientSocket?.Send(sendBytes);
        }


    }
}
