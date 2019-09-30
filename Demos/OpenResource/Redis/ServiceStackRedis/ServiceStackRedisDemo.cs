using ServiceStack.Redis;
using System;
using System.Configuration;
using System.Threading;

namespace Demos.OpenResource.Redis.ServiceStackRedis
{
    /// <summary>
    ///NuGet安装ServiceStack.Redis   
    ///C# Redis Client for the worlds fastest distributed NoSQL datastore. 
    /// </summary>
    public class ServiceStackRedisDemo
    {
        public static string MasterRedis { get; set; }

        public static string SlaveRedis { get; set; }

        public static readonly ServiceStackRedisDemo Instance;
        public PooledRedisClientManager PooledRedisClientManager { get; private set; }
        /// <summary>
        /// 读写客户端--Master
        /// </summary>
        public IRedisClient WriteReadRedisClient => PooledRedisClientManager.GetClient();
        /// <summary>
        /// 只读客户端--Slave
        /// </summary>
        public IRedisClient ReadOnlyRedisClient
        {
            get
            {
                return PooledRedisClientManager.GetReadOnlyClient();
            }
        }
        static ServiceStackRedisDemo()
        {
            MasterRedis = ConfigurationManager.AppSettings["ServiceStackMasterRedis"].ToString();
            SlaveRedis = ConfigurationManager.AppSettings["ServiceStackSlaveRedis"].ToString();
            Instance = new ServiceStackRedisDemo();
        }
        public ServiceStackRedisDemo()
        {
            PooledRedisClientManager = new PooledRedisClientManager(new string[] { MasterRedis },
                                                        new string[] { SlaveRedis },
                                                        new RedisClientManagerConfig
                                                        {
                                                            MaxWritePoolSize = 20,//“写”链接池链接数
                                                            MaxReadPoolSize = 20,//“读”链接池链接数
                                                            AutoStart = true,
                                                        });
        }

        void ServiceStackTest()
        {

            // 源码地址： https://github.com/search?q=ServiceStack&type=Repositories
            #region Redis 破解redis每小时只能添加6000次的限制
            /*
             * 1、NuGet安装好ServiceStack.Redis
                2、删除packages.config里ServiceStack.Text的配置
                3、移除ServiceStack.Text.dll的引用，用本地Dll目录下的Redis文件夹下的ServiceStack.Text.dll替换debug下的


                
                或者直接饮用 ServiceStackRedis里的所有dll
                或者下载源码按照网上说的修改源码，重新编译
             * */
            #endregion

            ////NuGet安装ServiceStack.Redis   
            ////C# Redis Client for the worlds fastest distributed NoSQL datastore. 
            //stackExchange
            //127.0.0.1:6379,password=123456,connectTimeout=1000,connectRetry=1,syncTimeout = 1000

            //RedisClient redis = new RedisClient("127.0.0.1", 6379);//redis服务IP和端口
            //redis.Db = 9;
            ////主Redis
            //RedisClient redisMster = new RedisClient("127.0.0.1", 6379,"fancky123",0);//redis服务IP和端口

            //var r = redisMster.Add<string>("master", "master1");

            ////只读从Redis
            //RedisClient redisSlave = new RedisClient("127.0.0.1", 6380);//redis服务IP和端口
            //redisSlave.Db = 0;
            ////报错："READONLY You can't write against a read only slave.”
            //var s = redisSlave.Add<string>("slave", "slave1");

            ////var r = redis.Add<string>("String1", "val1");
            //////切换数据库
            ////redis.Db = 2;
            ////redis.Add<string>("String1", "val1111");

            ////redis.Add<string>("String2", "val1222");
            ////redis.Remove("String1");
            ////redis.Add<string>("String1", "val1111222");
            ////redis.Remove("String2");
            //// 6000上线测试
            //for (int i = 0; i < 100000; i++)
            //{
            //    redis.Add<string>($"String{i}", i.ToString());

            //}
            //var readClient = ServiceStackRedis.Instance.ReadOnlyRedisClient;
            //readClient.Add<string>("sd", "dsss");
            var connStr = "redis://nunit:pass@host:1?ssl=true&db=0&connectTimeout=2&sendtimeout=3&receiveTimeout=4&idletimeoutsecs=5&NamespacePrefix=prefix.";
            var connStr1 = "redis://nunit:fancky123@127.0.0.1:6379?ssl=true&db=0&connectTimeout=2&sendtimeout=3&receiveTimeout=4&idletimeoutsecs=5&NamespacePrefix=prefix.";

            //127.0.0.1:6379?Client=nunit&Password=fancky123&Ssl=true&ConnectTimeout=2&SendTimeout=3&ReceiveTimeout=4&IdleTimeOutSecs=5&NamespacePrefix=prefix.
            //ToRedisEndpoint(connStr1);

            var master = ServiceStackRedisDemo.Instance.WriteReadRedisClient;
            //master.Add<string>("sd", "dsss");
            //master.Add<string>("sdExpire", "dsss",new TimeSpan(0,1,0));
            //var f=  master.Remove("lockKey");

            master.Remove("lockKey");
            master.Add<int>("lockKey", 1234);
            var counter = master.Get<int>("lockKey");
            master.AddItemToList("Key1", "1");
            master.AddItemToList("Key1", "2");
            master.AddItemToList("Key1", "3");
            master.Dispose();


            //for (int i = 0; i < 10; i++)
            //{
            //    using (var r1 = ServiceStackRedis.Instance.WriteReadRedisClient)
            //    {
            //        r1.Add<string>($"StrKey{i}", $"Val{i}");
            //    }

            //    //var r1 = ServiceStackRedis.Instance.WriteReadRedisClient;
            //    //r1.Add<string>($"StrKey{i}", $"Val{i}");
            //    //r1.Dispose();

            //}
            //ServiceStackRedis.Instance.TransactionTest();
            //ServiceStackRedis.Instance.LockTest();


        }

        public void Test()
        {
            StringTest();
            ListTest();
        }
        #region String
        public void StringTest()
        {
            WriteReadRedisClient.Set<string>("StringTest1", "st1");
            string val = ReadOnlyRedisClient.Get<string>("StringTest1");
            WriteReadRedisClient.Remove("StringTest1");
        }
        #endregion

        #region List
        public void ListTest()
        {

        }
        #endregion

        #region Hash
        public void HashTest()
        {

        }
        #endregion

        #region Set
        public void SetTest()
        {

        }
        #endregion

        #region SortedSet
        public void SortedSetTest()
        {

        }
        #endregion

        #region Tran
        public void TransactionTest()
        {
            string key = "rdtmultitest";
            using (WriteReadRedisClient)
            {
                using (var trans = WriteReadRedisClient.CreateTransaction())
                {
                    try
                    {
                        trans.QueueCommand(r => r.IncrementValue(key));
                        trans.QueueCommand(r => r.IncrementValue(key));
                        trans.QueueCommand(r => r.IncrementValue(key));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Lock
        public void LockTest()
        {
            string key = "lockKey";
            using (WriteReadRedisClient)
            {
                using (WriteReadRedisClient.AcquireLock("sdd"))
                {
                    try
                    {
                        var counter = WriteReadRedisClient.Get<int>(key);
                        Thread.Sleep(100);
                        WriteReadRedisClient.Set(key, counter + 1);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }
        #endregion

        #region DB
        public void DbIndex()
        {
            WriteReadRedisClient.Db = 12;
        }
        #endregion
    }
}
