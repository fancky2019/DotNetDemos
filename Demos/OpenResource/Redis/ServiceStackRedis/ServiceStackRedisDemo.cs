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
            MasterRedis= ConfigurationManager.AppSettings["MasterRedis"].ToString();
            SlaveRedis = ConfigurationManager.AppSettings["SlaveRedis"].ToString();
            Instance = new ServiceStackRedisDemo();
        }
        ServiceStackRedisDemo()
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

        #region String
        public void StringTest()
        {
            WriteReadRedisClient.Set<string>("StringTest1", "st1");
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
