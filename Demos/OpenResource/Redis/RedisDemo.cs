using Demos.OpenResource.Redis.ServiceStackRedis;
using StackExchange.Redis;
using System;
using System.Configuration;

namespace Demos.OpenResource.Redis.StackExchangeRedis
{
    class RedisDemo
    {
        public void Test()
        {

            ServiceStackTest();
        }

        void StackExchangeTest()
        {
            /**
         *
        一、启动Redis。点击start.bat。如果运行redis - server.exe，redis.windows.conf的配置将不起作用。
        二、修改Redis 数据库个数
            1、redis.windows.conf。
            2、找到databases,修改后面的数据。保存重启Redis。
            3、修改Redis Desktop Manager设置。
               a、点击Edit Connection Settings
               b、高级设置，设置 Database discovery limit值 2000。
               c、点击保存。
            4、Redis Desktop Manager点击Reload Server。
        三、持久化
        # RDB文件名
           dbfilename "dump.rdb"
           持久化--写入磁盘，下次重启时候从新读取，路径为redis路径
        四、redis集群
      **/

            //NuGet安装StackExchange.Redis

            // IDatabase db = redis.GetDatabase(1);//redis.GetDatabase(1)指定数据库1；如果不填写则默认为-1 
            //StackExchange.Redis 文档https://stackexchange.github.io/StackExchange.Redis/
            int[] a;//= new int[3];
            a = new int[] { 1, 2 };
            int[] b = { 1, 32 };



            string redisConnection = ConfigurationManager.AppSettings["RedisConnection"].ToString();
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
            IDatabase iDatabase = connectionMultiplexer.GetDatabase();
            //iDatabase.ListLeftPush("key1", JsonConvert.SerializeObject("fanckytest1"));
            //var listValye = JsonConvert.DeserializeObject(iDatabase.ListLeftPop("key1"));//会删除key，缓存list被删除
            //iDatabase.StringSet("key1", "123456");
            //var val = iDatabase.StringGet("key1");//不会清除key

            iDatabase.KeyDelete("key1");//删除key

            //切换数据库
            iDatabase = connectionMultiplexer.GetDatabase(9);


            //在Redis中存储常用的5种数据类型：String,Hash,List,SetSorted set
            //如果插入key相同的值，后插入的数据会覆盖前面的值

            #region  string
            iDatabase.StringSet("stringKey1", "123456");
            var val = iDatabase.StringGet("stringKey1");//不会清除key
            iDatabase.StringSet("stringKey1", "12345");
            var val2 = iDatabase.StringGet("stringKey1");//不会清除key

            #endregion


            var ex = iDatabase.KeyExists("stringKey132");



            #region Hash
            iDatabase.HashSet("hashKey1", "hashField", "张三");
            var hv = iDatabase.HashGet("hashKey1", "hashField");
            #endregion

            #region List

            /*
             * list是一个链表结构，主要功能是push,pop,获取一个范围的所有的值等，操作中key理解为链表名字。 
             * Redis的list类型其实就是一个每个子元素都是string类型的双向链表。我们可以通过push,pop操作从链表的头部或者尾部添加删除元素，
             * 这样list既可以作为栈，又可以作为队列。Redis list的实现为一个双向链表，即可以支持反向查找和遍历，更方便操作，不过带来了部分额外的内存开销，
             * Redis内部的很多实现，包括发送缓冲队列等也都是用的这个数据结构 
             */

            //栈的功能，LIFO
            iDatabase.ListLeftPush("listKey1", "张三");
            //iDatabase.ListLeftPush("listKey2", "张三");
            //队列功能，FIFO
            iDatabase.ListLeftPush("listKey1", "张三");
            iDatabase.ListRightPop("listKey2");
            var list = iDatabase.ListLeftPop("listKey2");
            #endregion

            #region Set无序集合
            /*
             它是string类型的无序集合。set是通过hash table实现的，添加，删除和查找,对集合我们可以取并集，交集，差集
             */

            iDatabase.SetAdd("redisKey4", "张三");
            var setVal = iDatabase.SetPop("redisKey4");

            #endregion

            #region  SetSorted 有序集合
            /*
             sorted set 是set的一个升级版本，它在set的基础上增加了一个顺序的属性，这一属性在添加修改.元素的时候可以指定，
             * 每次指定后，zset(表示有序集合)会自动重新按新的值调整顺序。可以理解为有列的表，一列存 value,一列存顺序。操作中key理解为zset的名字.
             */
            //iDatabase.SortedSetAdd("redisKey5", "张三");
            //var setVal = iDatabase.SetPop("redisKey4");
            #endregion


            Test test = new Test();

            //RedisKey rk = new RedisKey();
            //ek = "stringKey1";
            RedisKey t = "RedisKey";
            Display(test);



            Console.ReadLine();
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

        static void Display(Test test)
        {
            Console.WriteLine(test.Name);
        }
    }
    struct Test : IEquatable<Test>
    {
        public string Name { get; set; }

        public bool Equals(Test other)
        {
            throw new NotImplementedException();
        }
    }
}
