using DotNetty.Codecs;
using DotNetty.Codecs.Protobuf;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using NLog;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;


namespace TestService.Netty
{

    public class CommunicationClient
    {
        private static readonly Logger _nLog = NLog.LogManager.GetCurrentClassLogger();
        Bootstrap _bootstrap = new Bootstrap();
        IChannel _clientChannel = null;

        IPEndPoint _iPEndPoint = null;
        static MultithreadEventLoopGroup _group;
        volatile bool _closed = false;
        public event Action<string> ReceiveMsg;
        public event Action Connected;

        public string Port { get; set; }
        public string IP { get; set; }

        public string ClientNo { get; set; }
        static CommunicationClient()
        {
            _group = new MultithreadEventLoopGroup();
        }
        public CommunicationClient(string ip, string port)
        {
            this.IP = ip;
            this.Port = port;
        }
        public async void RunClientAsync()
        {

            //string _ip = "192.168.1.114";

            _iPEndPoint = new IPEndPoint(IPAddress.Parse(IP), int.Parse(Port));
            //_group = new MultithreadEventLoopGroup();
            try
            {
                _bootstrap.Group(_group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        //6s未读写就断开了连接。和java的一样设计
                        IdleStateHandler idleStateHandler = new IdleStateHandler(2, 2, 6);

                        pipeline.AddLast("timeout", idleStateHandler);

                        //pipeline.AddLast("ZDDecoder", new ZDDecoder());
                        //pipeline.AddLast("ZDEncoder", new ZDEncoder());

                        CommunicationClientHandler echoClientHandler = new CommunicationClientHandler(ReceiveMsg, this.ClientNo);
                        echoClientHandler.DisConnected += () =>
                          {
                              Connect(_iPEndPoint);
                          };
                        echoClientHandler.Connected += (content) =>
                        {
                            _nLog.Info("Connected to server......");
                            _clientChannel = content.Channel;
                            //MemoryData.Users[ClientNo].ConnectedTime = DateTime.Now;
                            Connected?.Invoke();
                        };
                        pipeline.AddLast("echo", echoClientHandler);
                    }));
                Connect(_iPEndPoint);
            }
            catch (Exception ex)
            {
                _nLog.Error(ex.ToString());
            }
        }

        public void SendMsg<T>(T t)
        {
            try
            {
                if (_clientChannel == null)
                {
                    _nLog.Info("连接异常，确保网络连接正常。");
                    return;
                }
                _clientChannel.WriteAndFlushAsync(t);
                //MemoryData.Users[ClientNo].SendLoginCmdTime = DateTime.Now;
                _nLog.Info($"Sent to Communication:{t.ToString()}");
            }
            catch (Exception ex)
            {
                _nLog.Error(ex.ToString());
            }
        }



        Task<IChannel> connectedChannel = null;
        public async void Connect(IPEndPoint iPEndPoint)
        {
            if (_closed || (_clientChannel != null && _clientChannel.Active))
            {
                return;
            }

            try
            {
                _nLog.Info("Connecting to server......");

                //connectedChannel =  _bootstrap.ConnectAsync(iPEndPoint);

                //_clientChannel = await _bootstrap.ConnectAsync(iPEndPoint);

                _bootstrap.ConnectAsync(iPEndPoint);
                //_nLog.Info("Connected to server......");
                //Connected?.Invoke();
            }
            catch (Exception ex)
            {
                _nLog.Error($"{ex.Message} - {ex.InnerException.Message}");
                if (_clientChannel != null)
                {
                    Thread.Sleep(2000);
                    Connect(iPEndPoint);
                }

            }



        }

        public async void Close()
        {
            _closed = true;
            if (_group != null)
            {
                if (_clientChannel != null)
                {
                    await _clientChannel.CloseAsync();
                }
                await _group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }


    }
}
