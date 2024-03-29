﻿using Demos.Common;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.Text;
using StackExchange.Redis;
using ServiceStack;

namespace Demos.OpenResource.Redis.ServiceStackRedis
{
    /// <summary>
    ///NuGet安装ServiceStack.Redis   
    ///C# Redis Client for the worlds fastest distributed NoSQL datastore. 
    ///github:https://github.com/ServiceStack/ServiceStack.Redis
    ///github:https://github.com/ServiceStack/ServiceStack.Text 6000次数限制
    ///修改ServiceStack.Text程序集内的源码 LicenseUtils.cs 内的类 FreeQuotas的常量RedisRequestPerHour
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
        public RedisClient WriteReadRedisClient => PooledRedisClientManager.GetClient() as RedisClient;
        /// <summary>
        /// 只读客户端--Slave
        /// </summary>
        public RedisClient ReadOnlyRedisClient
        {
            get
            {
                return PooledRedisClientManager.GetReadOnlyClient() as RedisClient;
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
            //DBIndex:0,
            //poolSizeMultiplier :默认20
            //PoolTimeout 4S
            PooledRedisClientManager = new PooledRedisClientManager(new string[] { MasterRedis },
                                                        new string[] { SlaveRedis },
                                                        new RedisClientManagerConfig
                                                        {
                                                            MaxWritePoolSize = 200,//“写”链接池链接数 ，设置大点不然很容易报池都在用造成超时的异常,下面英文的异常。
                                                            MaxReadPoolSize = 200,//“读”链接池链接数
                                                            AutoStart = true
                                                        }, 0, null, 4
                                                       );
            //Redis Timeout expired. The timeout period elapsed prior to obtaining a connection from the pool. 
            //This may have occurred because all pooled connections were in use.

            //PooledRedisClientManager.PoolTimeout  默认两秒 上面改成4秒
        }

        void WriteSixThoundsPerHourLimitTest()
        {
            var redis = ServiceStackRedisDemo.Instance.WriteReadRedisClient;
            // 6000上线测试
            for (int i = 0; i < 100000; i++)
            {
                //redis.Add<string>($"String{i}", i.ToString());
                redis.SetValue($"String{i}", i.ToString());
            }
        }
        void ServiceStackTest()
        {

            // 源码地址： https://github.com/ServiceStack/ServiceStack.Redis
            #region Redis 破解redis每小时只能添加6000次的限制
            //*github:https://github.com/ServiceStack/ServiceStack.Text
            //修改ServiceStack.Text程序集内的源码 LicenseUtils.cs 内的类
            /*
             *         public static class FreeQuotas
                    {
                        public const int ServiceStackOperations = 10;
                        public const int TypeFields = 20;
                        public const int RedisTypes = 20;
                        public const int RedisRequestPerHour = 6000;=>  public const int RedisRequestPerHour = int.MaxValue;
                        public const int OrmLiteTables = 10;
                        public const int AwsTables = 10;
                        public const int PremiumFeature = 0;
                    }
             */
            /*github:https://github.com/ServiceStack/ServiceStack.Text
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

        /// <summary>
        /// 具体使用可参照源码的测试：ServiceStack.Redis.Tests工程文件。
        /// </summary>
        public void Test()
        {
            WriteSixThoundsPerHourLimitTest();
            //Utility();
            //ReadOnlyRedisClient.Db = 1;
            //StringTest();
            //BitMap();
            //ListTest();
            //HashTest();
            //SetTest();
            //SortedSetTest();
            //ExpiryKey();
            //TransactionTest();
            //LockTest();
            //BatchInsert();
            //RedisQueue();
            //PubSub();
            //ExpireCallBack();
        }

        /// <summary>
        /// http://www.redis.cn/
        /// 其他的可参见redis命令
        /// </summary>
        private void UtilityMethod()
        {
            /*redis中允许模糊查询的有3个通配符，分别是：*，?，[]
             * 和正则表达式里的含义一样
              *：通配任意多个字符
              ?：通配单个字符
             []：通配括号内的某一个字符
             */
            //ksy不存在返回0，存在返回1
            var re = WriteReadRedisClient.Exists("StringTest1");
            WriteReadRedisClient.FlushDb();//清空当前数据库
            WriteReadRedisClient.Db = 0;

        }

        #region String
        public void StringTest()
        {
            // 数据结构
            //StringRedisKey1  StringValue1
            //StringRedisKey1  StringValue2
            //StringRedisKey1  StringValue3
            //    *                *
            //    *                *
            //    *                *




            //写
            WriteReadRedisClient.Set<string>("StringKey1", "StringValue1");
            WriteReadRedisClient.SetValue("StringKey2", "StringValue2");
            //更新：覆盖原有值
            WriteReadRedisClient.SetValue("StringKey2", "StringValue3");

            WriteReadRedisClient.SetValue("StringExpireKey", "StringExpireValue", new TimeSpan(0, 0, 30));
            //Thread.Sleep(10 * 1000);
            WriteReadRedisClient.SetValue("StringExpireKey", "StringExpireValue1");//key存在且无过期时间返回-1
            //返回time to live 单位ms
            /*
             * 如果key不存在返回-2
               如果key存在且无过期时间返回-1
             * */
            var ttl1 = WriteReadRedisClient.PTtl("StringExpireKey");
            //返回time to live 单位s
            WriteReadRedisClient.Ttl("StringExpireKey");


            WriteReadRedisClient.SetValue("StringExpireKey1", "StringExpireValue1", new TimeSpan(0, 0, 30));
            Thread.Sleep(10 * 1000);
            //覆盖key的值：不会更新过期时间
            WriteReadRedisClient.SetRange("StringExpireKey1", 0, "StringExpireValue2".ToUtf8Bytes());
            var ttl2 = WriteReadRedisClient.PTtl("StringExpireKey1");


            //如果key 不存在就设置值返回true，如果key存在就返回false也不更新值。
            var reee = WriteReadRedisClient.SetValueIfNotExists("ktttttt", "ddddd");
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            WriteReadRedisClient.SetValues(keyValuePairs);
            WriteReadRedisClient.SetAll(keyValuePairs);

            //不会更新过期时间
            WriteReadRedisClient.SetRange("StringSetRangeKey", 0, "StringSetRangeKeyValue".ToUtf8Bytes());

            string val = ReadOnlyRedisClient.Get<string>("StringKey2");




            //设置key 并设定过期时间
            WriteReadRedisClient.SetValue("StringExpiryKey1", "StringExpiryValue1", TimeSpan.FromSeconds(20));
            //读
            ReadOnlyRedisClient.ContainsKey("StringKey2");

            List<string> keys = ReadOnlyRedisClient.SearchKeys("String*");
            //删
            long re = WriteReadRedisClient.Del("StringKey1");
            //remove 内部调用的是Del。
            bool re1 = WriteReadRedisClient.Remove("StringKey2");

            var len = WriteReadRedisClient.StrLen("StringKey2");
        }
        #endregion

        #region BitMap

        private void BitMap()
        {
            //活动签到记录可以用BitMap表示,key(userID)、距离活动开始天数（offset）、签到天数（value 1)
            /*
             * BitMap 实际还是string类型的存储结构
             * 初始最小1个Byte 8位
             * 
             * offset:>=1
             * value:1
             */
            WriteReadRedisClient.SetBit("BitMapKey11", 3, 1);//在二进制的第三个位置设置1
            long b = WriteReadRedisClient.GetBit("BitMapKey11", 3);//获取偏移位置上的bit
            long bitCount = WriteReadRedisClient.BitCount("BitMapKey11");
        }
        #endregion

        #region List
        public void ListTest()
        {
            //数据结构
            //ListRedisKey1   ListValue1
            //                ListValue2
            //                ListValue3
            //                    * 
            //                    * 
            //                    * 
            //ListRedisKey2   ListValue1
            //                ListValue2
            //                ListValue3
            //                    * 
            //                    * 
            //                    * 



            //写
            //内部调用 RPush
            WriteReadRedisClient.AddItemToList("ListKey1", "ListValue1");
            WriteReadRedisClient.AddItemToList("ListKey1", "ListValue2");
            WriteReadRedisClient.AddItemToList("ListKey1", "ListValue3");
            WriteReadRedisClient.AddItemToList("ListKey1", "ListValue4");
            WriteReadRedisClient.RPush("ListKey1", Encoding.UTF8.GetBytes("ListValue5"));
            WriteReadRedisClient.LPush("ListKey1", Encoding.UTF8.GetBytes("ListValue6"));

            WriteReadRedisClient.AddRangeToList("ListKey1", new List<string> { "ListValue7", "ListValue8" });

            WriteReadRedisClient.AddItemToList("ListKey2", "ListValue1");
            WriteReadRedisClient.AddItemToList("ListKey2", "ListValue2");
            WriteReadRedisClient.AddItemToList("ListKey2", "ListValue3");

            //读
            //LILO  左出队  栈
            string listLeftValue = Encoding.UTF8.GetString(WriteReadRedisClient.LPop("ListKey1"));
            //FIFO  右出队  队列
            string listRightValue = Encoding.UTF8.GetString(WriteReadRedisClient.RPop("ListKey1"));
            //WriteReadRedisClient.BlockingPopItemFromList("ListKey1", TimeSpan.FromSeconds(2));
            var len = WriteReadRedisClient.LLen("ListKey1");
            //删除
            WriteReadRedisClient.Remove("ListKey1");
            WriteReadRedisClient.Remove("ListKey2");
        }
        #endregion

        #region Hash
        public void HashTest()
        {


            // 数据结构
            //HashSetRedisKey1  HashSetKey1 HashSetValue1
            //                  HashSetKey2 HashSetValu2
            //                  HashSetKey3 HashSetValu3
            //                       *            *
            //                       *            *
            //                       *            *
            //HashSetRedisKey2  HashSetKey1 HashSetValue1
            //                  HashSetKey2 HashSetValu2
            //                  HashSetKey3 HashSetValu3
            //                      *             *
            //                      *             *
            //                      *             *


            //写
            WriteReadRedisClient.SetEntryInHash("RedisHashKey1", "HashKey1", "HashValue1");
            WriteReadRedisClient.SetEntryInHash("RedisHashKey1", "HashKey2", "HashValue2");
            WriteReadRedisClient.SetEntryInHash("RedisHashKey1", "HashKey3", "HashValue3");

            WriteReadRedisClient.SetEntryInHash("RedisHashKey2", "HashKey1", "HashValue1");
            WriteReadRedisClient.SetEntryInHash("RedisHashKey2", "HashKey2", "HashValue2");


            //读
            var hashValue = WriteReadRedisClient.GetValueFromHash("RedisHashKey1", "HashKey2");
            //获取该redishashkey的所有值
            var hashValues = WriteReadRedisClient.GetHashValues("RedisHashKey1");

            //删
            //删除一个
            WriteReadRedisClient.RemoveEntryFromHash("RedisHashKey1", "HashKey2");
            //删除整个Key的数据
            WriteReadRedisClient.Remove("RedisHashKey1");
            WriteReadRedisClient.Remove("RedisHashKey2");

            WriteReadRedisClient.SetEntryInHash("RedisHashKey3", "HashKey1", "HashValue1");
            //存在1,不存在0
            var exist = WriteReadRedisClient.HExists("RedisHashKey3", Encoding.UTF8.GetBytes("HashKey1"));
            var notExist = WriteReadRedisClient.HExists("RedisHashKey2", Encoding.UTF8.GetBytes("HashKey1"));
            var existKey = WriteReadRedisClient.Exists("RedisHashKey3");
        }
        #endregion

        #region Set
        public void SetTest()
        {

            //数据结构
            //SetRedisKey1    SetValue1
            //                SetValue2
            //                SetValue3
            //                    * 
            //                    * 
            //                    * 
            //SetRedisKey2    SetValue1
            //                SetValue2
            //                SetValue3
            //                    * 
            //                    * 
            //                    * 


            //写
            WriteReadRedisClient.AddItemToSet("SetRedisKey1", "SetRedisValue1");
            //重复添加只能添加一个。
            WriteReadRedisClient.AddItemToSet("SetRedisKey1", "SetRedisValue1");
            WriteReadRedisClient.AddRangeToSet("SetRedisKey1", new List<string> { "SetRedisValue2", "SetRedisValue3" });
            WriteReadRedisClient.AddItemToSet("SetRedisKey1", "SetRedisValue4");

            WriteReadRedisClient.AddItemToSet("SetRedisKey2", "SetRedisValue1");
            WriteReadRedisClient.AddItemToSet("SetRedisKey2", "SetRedisValue2");
            WriteReadRedisClient.AddItemToSet("SetRedisKey2", "SetRedisValue4");
            WriteReadRedisClient.AddItemToSet("SetRedisKey2", "SetRedisValue3");
            WriteReadRedisClient.AddItemToSet("SetRedisKey2", "SetRedisValue5");

            //交集
            var intersectingMembers = WriteReadRedisClient.GetIntersectFromSets("SetRedisKey1", "SetRedisKey2");
            WriteReadRedisClient.StoreIntersectFromSets("IntersectSetRedisKey1", "SetRedisKey1", "SetRedisKey2");
            //并集
            var unionMembers = WriteReadRedisClient.GetUnionFromSets("SetRedisKey1", "SetRedisKey2");
            WriteReadRedisClient.StoreUnionFromSets("UnionSetRedisKey1", "SetRedisKey1", "SetRedisKey2");
            //差集：第一个参数减去第二个参数（fromSetId - withSetIds）
            var diffMembers = WriteReadRedisClient.GetDifferencesFromSet("SetRedisKey1", "SetRedisKey2");
            var diffMembers2 = WriteReadRedisClient.GetDifferencesFromSet("SetRedisKey2", "SetRedisKey1");
            WriteReadRedisClient.StoreDifferencesFromSet("DifferenceSetRedisKey1", "SetRedisKey2", "SetRedisKey1");
            //读
            var members = WriteReadRedisClient.GetAllItemsFromSet("SetRedisKey1");

            //删
            WriteReadRedisClient.RemoveItemFromSet("SetRedisKey1", "SetRedisValue3");

            WriteReadRedisClient.Remove("SetRedisKey1");
            WriteReadRedisClient.Remove("SetRedisKey2");
            WriteReadRedisClient.Remove("IntersectSetRedisKey1");
            WriteReadRedisClient.Remove("UnionSetRedisKey1");
            WriteReadRedisClient.Remove("DifferenceSetRedisKey1");
        }
        #endregion

        #region SortedSet
        public void SortedSetTest()
        {

            //数据结构
            //                                           Score
            //SortedSetRedisKey1    SortedSetValue1        1
            //                      SortedSetValue2        2
            //                      SortedSetValue3        3
            //                           *                 * 
            //                           * 
            //                           * 
            //SortedSetRedisKey2    SortedSetValue1        1
            //                      SortedSetValue2        2
            //                      SortedSetValue3        3
            //                           *                 * 
            //                           *                 * 
            //                           *                 * 


            int i = 0;
            //写
            //Redis.AddItemToSortedSet(SetId, x, 1)
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey1", "SortedSetRedisValue1", ++i);
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey1", "SortedSetRedisValue2", ++i);
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey1", "SortedSetRedisValue3", ++i);
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey1", "SortedSetRedisValue4", ++i);

            //
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey2", "SortedSetRedisValue1", Utility.GetTimeStamp());
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey2", "SortedSetRedisValue2", Utility.GetTimeStamp());
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey2", "SortedSetRedisValue3", Utility.GetTimeStamp());
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey2", "SortedSetRedisValue4", Utility.GetTimeStamp());
            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey2", "SortedSetRedisValue5", Utility.GetTimeStamp());


            WriteReadRedisClient.AddItemToSortedSet("SortedSetRedisKey3", "SortedSetRedisValue1");
            //读
            var members = WriteReadRedisClient.GetAllItemsFromSortedSet("SortedSetRedisKey1");


            //最小
            var minRedisValue = WriteReadRedisClient.GetRangeFromSortedSet("SortedSetRedisKey2", 0, 0)[0];
            //最大
            var maxRedisValue = WriteReadRedisClient.GetRangeFromSortedSetDesc("SortedSetRedisKey2", 0, 0)[0];

            var dic1 = WriteReadRedisClient.GetRangeWithScoresFromSortedSet("SortedSetRedisKey2", 0, 0);
            var dic = WriteReadRedisClient.GetRangeWithScoresFromSortedSetDesc("SortedSetRedisKey2", 0, 0);
            var sortedSetRedisValue = dic.Keys.First();
            var sortedSetRedisScore = dic.Values.First();
            //交集
            WriteReadRedisClient.StoreIntersectFromSortedSets("IntersectSortedSetRedisKey1", "SortedSetRedisKey1", "SortedSetRedisKey2");
            //并集
            WriteReadRedisClient.StoreUnionFromSortedSets("UnionSortedSetRedisKey1", "SortedSetRedisKey1", "SortedSetRedisKey2");
            //差集：不支持差集



            //删除
            //根据时间戳,先入队的先出队
            var valL = WriteReadRedisClient.PopItemWithLowestScoreFromSortedSet("SortedSetRedisKey2");
            //最大值
            var valH = WriteReadRedisClient.PopItemWithHighestScoreFromSortedSet("SortedSetRedisKey2");
            WriteReadRedisClient.RemoveItemFromSortedSet("SortedSetRedisKey1", "SortedSetRedisValue4");
            WriteReadRedisClient.Remove("SortedSetRedisKey1");
            WriteReadRedisClient.Remove("SortedSetRedisKey2");
            WriteReadRedisClient.Remove("IntersectSortedSetRedisKey1");
            WriteReadRedisClient.Remove("UnionSortedSetRedisKey1");

        }
        #endregion

        #region Expiry
        /// <summary>
        /// key 到期redis会删除
        /// </summary>
        private void ExpiryKey()
        {
            WriteReadRedisClient.SetValue("StringExpiryKey1", "StringExpiryValue1", TimeSpan.FromSeconds(20));

            var ttl = WriteReadRedisClient.GetTimeToLive("StringExpiryKey1");

            WriteReadRedisClient.AddItemToList("listExpiryKey", "list");
            WriteReadRedisClient.Expire("listExpiryKey", 20);

            WriteReadRedisClient.SetEntryInHash("RedisHashKeyExpiryKey", "HashKey1", "HashValue1");
            WriteReadRedisClient.Expire("RedisHashKeyExpiryKey", 20);

            WriteReadRedisClient.AddItemToSet("SetRedisKeyExpiryKey", "SetRedisValue1");
            WriteReadRedisClient.Expire("SetRedisKeyExpiryKey", 20);
        }
        #endregion

        #region Tran
        public void TransactionTest()
        {
            string key = "TransactionTestKey";
            //redis 事务不具有隔离，利用乐观锁CAS简单处理。
            //添加CAS条件
            string lockKey = "TransactionLockeStrKey";
            string lockeStrKeyValue = Guid.NewGuid().ToString();
            using (WriteReadRedisClient)
            {
                using (var trans = WriteReadRedisClient.CreateTransaction())
                {

                    try
                    {
                        if (WriteReadRedisClient.Exists(lockKey) == 0)//如果Key不存在
                        {
                            WriteReadRedisClient.SetValue(lockKey, lockeStrKeyValue);
                        }

                        WriteReadRedisClient.SetValue(lockKey, lockeStrKeyValue);

                        // 命令前被放入队列缓存，并不会被实际执行，
                        trans.QueueCommand(r => r.IncrementValue(key));
                        trans.QueueCommand(r => r.IncrementValue(key));
                        trans.QueueCommand(r => r.IncrementValue(key));

                        if (WriteReadRedisClient.Get<string>(lockKey) == lockeStrKeyValue)
                        {
                            var success = trans.Commit();
                        }

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
        /// <summary>
        /// 加锁逻辑
        /// 如果不指定锁时间，默认365天。
        /// 如果redis不存在锁的key，就写入key，跳出循环，也就获得锁。
        /// 如果redis存在锁的key，while(true)sleep++循环一直循环，等待上一个锁任务完成RedisLock调用Dispose()时候
        /// Remove(key)跳出循环获得锁。
        /// while()循环timeOut时间。
        /// </summary>
        public void LockTest()
        {
            string key = "lockKey";
            using (WriteReadRedisClient)
            {
                //获取锁，获取之前一直循环等待
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

        #region 批量
        /// <summary>
        /// 批量插入10W耗时900ms左右
        /// </summary>
        private void BatchInsert()
        {
            var listKey = "redisQueue";
            Task.Run(() =>
            {

                var producerClient = PooledRedisClientManager.GetClient() as RedisClient;
                producerClient.Db = 13;
                producerClient.FlushDb();

                List<string> list = new List<string>();
                StopwatchHelper.Instance.Start();
                for (int i = 0; i < 100000; i++)
                {
                    list.Add($"message - {i}");

                }
                producerClient.AddRangeToList(listKey, list);
                StopwatchHelper.Instance.Stop();
                Console.WriteLine(StopwatchHelper.Instance.Stopwatch.ElapsedMilliseconds);
            });
        }
        #endregion

        #region 生产者消费者队列
        private void RedisQueue()
        {
            try
            {


                var listKey = "redisQueue";
                Task.Run(() =>
                {

                    var producerClient = PooledRedisClientManager.GetClient() as RedisClient;
                    producerClient.Db = 13;
                    producerClient.FlushDb();
                    var message = "message";
                    StopwatchHelper.Instance.Start();
                    for (int i = 0; i < 100; i++)
                    {
                        message = $"message - {i}";
                        producerClient.LPush(listKey, Encoding.UTF8.GetBytes(message));
                    }
                    StopwatchHelper.Instance.Stop();
                    Console.WriteLine(StopwatchHelper.Instance.Stopwatch.ElapsedMilliseconds);
                });
                Thread.Sleep(1000);

                Task.Run(() =>
                {
                    var consumerClient = PooledRedisClientManager.GetClient() as RedisClient;
                    consumerClient.Db = 13;
                    while (true)
                    {

                        var len = consumerClient.LLen(listKey);
                        //没有数据就阻塞
                        var re = consumerClient.BRPop(listKey, 0);
                        var key = Encoding.UTF8.GetString(re[0]);//listKey
                        var value = Encoding.UTF8.GetString(re[1]);
                        Console.WriteLine($"{value}");
                    }

                });
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region
        private void PubSub()
        {
            string channelName = "ocgChannel";
            Random random = new Random();

            Task.Run(() =>
            {
                var consumerClient = PooledRedisClientManager.GetClient() as RedisClient;

                //创建订阅
                IRedisSubscription subscription = consumerClient.CreateSubscription();
                //接受到消息时
                subscription.OnMessage = (channel, msg) =>
                {
                    Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}");
                    Console.WriteLine($"频道订阅数目：{subscription.SubscriptionCount}");

                };
                //订阅频道时
                subscription.OnSubscribe = (channel) =>
                {
                    Console.WriteLine("订阅客户端：开始订阅" + channel);
                };
                //取消订阅频道时
                subscription.OnUnSubscribe = (a) => { Console.WriteLine("订阅客户端：取消订阅"); };

                //订阅频道
                subscription.SubscribeToChannels(channelName);
            });
            Thread.Sleep(100);
            Task.Run(() =>
            {
                var redisClient = PooledRedisClientManager.GetClient() as RedisClient;
                //redisClient.Db = 13;
                //redisClient.FlushDb();
                for (int i = 0; i < 100; i++)
                {
                    redisClient.PublishMessage(channelName, $"message - {i}");
                    Thread.Sleep(random.Next(20, 50));
                }

            });

        }
        #endregion

        #region
        private void ExpireCallBack()
        {


            var redisClient = PooledRedisClientManager.GetClient() as RedisClient;
            redisClient.Db = 13;
            redisClient.FlushDb();


            //订阅
            //var subscription = redisClient.CreateSubscription();
            //subscription.OnSubscribe = channel =>
            //{
            //    Console.WriteLine("开始订阅 '{0}'", channel);
            //};
            //subscription.OnUnSubscribe = channel =>
            //{
            //    Console.WriteLine("取消订阅 '{0}'", channel);
            //};
            //subscription.OnMessage = (channel, msg) =>
            //{
            //    Console.WriteLine("过期的ID '{0}' -- 通道名称 '{1}'", msg, channel);
            //};
            //subscription.SubscribeToChannels("__keyevent@13__:expired");





            redisClient.SetValue("StringExpiryKey1", "StringExpiryValue1", TimeSpan.FromSeconds(5));

            var redisPubSub = new RedisPubSubServer(PooledRedisClientManager, "channel-1", "channel-2")
            {
                OnMessage = (channel, msg) => "Received '{0}' from '{1}'".Print(msg, channel)
            }.Start();



        }
        #endregion
    }
}
