﻿using Common;
using Demos.Common;
using Demos.Demos2018.RabbitMQ;
using Demos.Demos2018.SynchronizationDemo;
using Demos.Demos2019;
using Demos.Demos2019.Encrypt;
using Demos.Demos2019.Forms;
using Demos.Demos2019.Proxy;
using Demos.Demos2019.Subjects;
using Demos.Demos2020;
using Demos.Demos2021;
using Demos.Model;
using Demos.OpenResource.Dapper;
using Demos.OpenResource.DotNettyDemo;
using Demos.OpenResource.HPSocket;
using Demos.OpenResource.Json;
using Demos.OpenResource.Jwt;
using Demos.OpenResource.Kafka;
using Demos.OpenResource.MessagePackDemo;
using Demos.OpenResource.ProtoBuf;
using Demos.OpenResource.Redis;
using Demos.OpenResource.Redis.ServiceStackRedis;
using Demos.OpenResource.Redis.StackExchangeRedis;
using Demos.OpenResource.SnowFlakeDemo;
using Demos.OpenResource.SQLite;
using Demos.Demos2021.ExtendImplement;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demos.Demos2018
{
    class Program
    {
        private static readonly NLog.Logger _nLog = NLog.LogManager.GetCurrentClassLogger();
        //[STAThread]
        static void Main(string[] args)
        {

            try
            {


                //AutoStart(true);
                //new AutoStartProgramDemo().Test();
                //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                  {
                      Console.WriteLine(e.ExceptionObject.ToString());
                      Console.WriteLine(e.ToString());
                      _nLog.Error(e.ToString());
                      _nLog.Error(e.ExceptionObject.ToString());

                  };

                Application.ThreadException += (sender, e) =>
                      {
                          Console.WriteLine(e.ToString());
                          _nLog.Error(e.ToString());
                      };



                #region Demos2018
                //new ParamsDemo().Test();
                // string str = Test().Result;
                //new AdoTest().Test();
                new LambdaTest().Test();

                //new TClassTest<Product>().Test();
                //new LockDemo().Test();

                #region SynchronizationDemo
                //   new AutoResetEventTest().Test();
                // new ManualResetEventTest().Test();
                //  new ProducerConsumer(100).Test();

                //   new ProducerConsumerTPS(int.MaxValue, 10).Test();
                //   new ProducerConsumerTPSDemo().Test();
                #endregion

                //new BlockingCollectionDemo().Test();

                // new AttributeDemo().Test();
                //new ImplicitExplicitDemo().Test();

                //new RabbitMQTest().Test();
                //new TryCatchFinallyReturnDemo().Test();


                //new RedisDemo().Test();

                #endregion

                #region Demos2019
                //new EqualsOperatorDemo().Test();
                //new ThreadDemo().Test();
                //new StringDemo().Test();
                new CollectionDemo().Test();
                // new HPSocketDemo().Test();
                //new CharDemo().Test();

                //new SubjectTest().Test();
                //new DivisionDemo().Test();
                //   new StackTraceDemo().Test();
                //new ProxyDemo().Test();
                // new StackExchangeRedisDemo().Test();

                //   new NewOverrieDemo().Test();
                //  new FrmTest().ShowDialog();
                //new RegexDemo().Test();
                //new Demos2019.Http.HttpTest().Test();
                //new TextTest().Test();
                //new StopwatchDemo().Test();
                //new JWTDemo().Test();
                //new FormatDemo().Test();

                //  new DotNetEightDemo().Test
                //new ClassExcuteSequenceDemo().Test();

                //new IndexDemo().Test();
                //    new ConstructorDemo().Test();
                //new ListSortCompare().Test();
                //ObjectClone.Test();

                //new ThreadTimeOutDemo().Test();
                //new ReferenceDemo().Test();
                //new AsyncDemo().Test();
                //new StaticClassTest();
                //new InDemo().Test();

                //new CovarianceContravarianceDemo().Test();
                //new HashCodeDemo().Test();
                //new FileUsing().Test();

                //new IteratorsDemo().Test();

                //new IndexDemo().Test();

                //new JsonDemo().Test();
                //new KafkaDemo().Test();

                //new KeyValuePairDemo().Test();

                //new StackExchangeDemo().Test();
                //new ServiceStackRedisDemo().Test();
                //new SpinLockDemo().Test();

                //new InterlockedDemo().Test();
                //new SemaphoreDemo().Test();
                //new VolatileDemo().Test();

                //   new SpinWaitDemo().Test();
                //new QueueMiddleWareDemo().Test();

                //new LockDemo().Test();
                //new DateTimeDemo().Test();
                //  new EncryptDemo().Test();

                //new ParamsDemo().Test();
                //BufferLog.Test();
                //LogManager.Test();
                //LogManager.GetLogger("log").Test();
                //new ReflectionDemo().Test();
                //new ThreadLocalDemo().Test();
                //new ParallelDemo().Test();

                //new TimerDemo().Test();

                //new NLogDemo().Test();

                //Task.Run(() =>
                //{
                //    new TCPServerDemo().Test();
                //});
                //Task.Run(() =>
                //{
                //    Thread.Sleep(1000);
                //    new TcpClientDemo().Test();
                //});
                //Task.Run(() =>
                //{
                //    Thread.Sleep(1000);
                //    new UDPServerDemo().Test();
                //});
                //Task.Run(() =>
                //{

                //    new UDPClientDemo().Test();
                //});

                //Task.Run(() =>
                //{
                //    Thread.Sleep(1000);
                //    new SocketServerDemo().Test();
                //});
                //Task.Run(() =>
                //{

                //    new SocketClientDemo().Test();
                //});
                //Log.Test();

                //new TaskDemo().Test();

                #endregion

                #region Demos2020

                //new Demos.Demos2020.CSVComma().Test();

                //new NettyTest().Test();

                //new MessagePackDemo().Test();

                //new MessagePackCli().Test();


                //new BitConverterDemo().Test();

                //new NettyUDPServer().Test();
                //new NettyUDPClient().Test();
                //new YieldReturnDemo().Test();
                //var id1 = new SnowFlake(1, 0).Test();
                //var id2 = new FanckySnowFlake(1, DateTime.Parse("2020-01-01")).Test();
                //var id2 = new WorkerIDSnowFlake(1, DateTime.Parse("2020-01-01")).Test();
                //var id2 = new ConfiguredSequenceSnowFlake(1, 12, DateTime.Parse("2020-01-01")).Test();

                //new SQLiteDemo().Test();
                //new DapperDemo().Test();
                //new ServiceStackJsonDemo().Test();

                //new ConcurrentDictionaryDemo().Test();

                //new ProtoBufNetDemo().Test();
                //new ProtobufGoogle().Test();
                //new CirculationDemo().Test();

                //new ByteBase64String().Test();


                #endregion

                #region Demos2021
                //new ThreadExceptionDemo().Test();
                //new ExtendImplementDemo().Test();
                //new ModRemDemo().Test();
                //new Demos.OpenResource.Redis.StackExchangeRedis.RedlockDemo().Test();

                #endregion



                Console.WriteLine("Main Completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _nLog.Error(ex.ToString());
            }
            Console.ReadLine();
        }


        /// <summary>  
        /// 修改程序在注册表中的键值  
        /// </summary>  
        /// <param name="isAuto">true:开机启动,false:不开机自启</param> 
        public static void AutoStart(bool isAuto)
        {
            try
            {
                if (isAuto == true)
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //R_run.SetValue("应用名称", Application.ExecutablePath);
                    R_run.SetValue("Demos", Application.ExecutablePath);
                    R_run.Close();
                    R_local.Close();
                }
                else
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //R_run.DeleteValue("应用名称", false);
                    R_run.DeleteValue("Demos", false);
                    R_run.Close();
                    R_local.Close();
                }

                //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
            }
            catch (Exception)
            {
                //MessageBoxDlg dlg = new MessageBoxDlg();
                //dlg.InitialData("您需要管理员权限修改", "提示", MessageBoxButtons.OK, MessageBoxDlgIcon.Error);
                //dlg.ShowDialog();
            }
        }
    }
}
