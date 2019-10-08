using Demos.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Redis.StackExchangeRedis
{
    /// <summary>
    /// Redis:Key-Value 的内存数据库
    /// 一个RedisKey  只能有一种数据类型
    /// 
    /// github:https://github.com/StackExchange/StackExchange.Redis
    /// </summary>
    class StackExchangeDemo
    {

        public void Test()
        {
            //RedisValue
            //explicit operator
            //  implicit operator

            // StackExchangeTest();

            //StringOperaton();
            //HashOperaton();
            //ListOperaton();
            //SetOperaton();
            SortedSetOperaton();
            //ExpiryKey();
            //Increment();
            //Transaction();
        }
        void StackExchangeTest()
        {
            /**
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




            Console.ReadLine();
        }

        IDatabase GetDatabase()
        {
            string redisConnection = ConfigurationManager.AppSettings["StackExchangeMasterRedis"].ToString();
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
            return connectionMultiplexer.GetDatabase();
        }

        #region  string

        private void StringOperaton()
        {
            // 数据结构
            //StringRedisKey1  StringValue1
            //StringRedisKey1  StringValue2
            //StringRedisKey1  StringValue3
            //    *                *
            //    *                *
            //    *                *



            IDatabase iDatabase = GetDatabase();
            //写
            iDatabase.StringSet("stringKey1", "123456");
            iDatabase.StringSet("stringKey2", "stringValue2");
            iDatabase.StringSet("stringKey3", "stringValue3");
            //读
            var val = iDatabase.StringGet("stringKey1");
            iDatabase.StringSet("stringKey1", "stringVal1");
            //写入已存在的Key会覆盖原来的值，和java的hashmap一样。
            var val2 = iDatabase.StringGet("stringKey1");
            var valEmpty = iDatabase.StringGet("stringKey111111");
            var isnull = valEmpty.IsNull;//true
            var isEmpty = valEmpty.HasValue;//false
            iDatabase.StringSet("stringKey2", "stringVal2");
            //存在
            var re = iDatabase.KeyExists("stringKey1");
            //删除

            iDatabase.KeyDelete("stringKey1");
            iDatabase.KeyDelete("stringKey2");
            iDatabase.KeyDelete("stringKey3");
        }
        #endregion

        #region Hash
        private void HashOperaton()
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

            IDatabase iDatabase = GetDatabase();


            //写
            iDatabase.HashSet("hashKey1", "hashField1", "张三");
            //写入已存在的Key会覆盖原来的值
            iDatabase.HashSet("hashKey1", "hashField1", "李四");
            iDatabase.HashSet("hashKey2", "hashField1", "HashValue1");
            iDatabase.HashSet("hashKey2", "hashField2", "HashValue2");
            iDatabase.HashSet("hashKey2", "hashField3", "HashValue1");
            //读  redis key =hashKey1 ,hashkey=hashField的数据
            var hv = iDatabase.HashGet("hashKey1", "hashField");
            var hsEmpty = iDatabase.HashGet("hashKey1", "hashField22222");
            var isnull = hsEmpty.IsNull;//true
            var isEmpty = hsEmpty.HasValue;//false

            //redis key=hashKey2的所有hashset数据
            HashEntry[] hashSet2 = iDatabase.HashGetAll("hashKey2");

            var re = iDatabase.KeyExists("hashKey22222");
            //不存在返回空数组
            HashEntry[] hashEntryEmpty = iDatabase.HashGetAll("hashKey22222");
            int length = hashEntryEmpty.Length;
            //转换成String类型字典
            var dict = hashSet2.ToStringDictionary();


            //删
            iDatabase.HashDelete("hashKey2", "hashField2");

            iDatabase.KeyDelete("hashKey1");
            iDatabase.KeyDelete("hashKey2");

        }
        #endregion

        #region List
        private void ListOperaton()
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


            /*
             * list是一个链表结构，主要功能是push,pop,获取一个范围的所有的值等，操作中key理解为链表名字。 
             * Redis的list类型其实就是一个每个子元素都是string类型的双向链表。我们可以通过push,pop操作从链表的头部或者尾部添加删除元素，
             * 这样list既可以作为栈，又可以作为队列。Redis list的实现为一个双向链表，即可以支持反向查找和遍历，更方便操作，不过带来了部分额外的内存开销，
             * Redis内部的很多实现，包括发送缓冲队列等也都是用的这个数据结构 
             */

            //栈的功能，LIFO
            IDatabase iDatabase = GetDatabase();
            iDatabase.ListLeftPush("listKey1", "张三");
            iDatabase.ListLeftPop("listKey1");
            //队列功能，FIFO
            iDatabase.ListLeftPush("listKey2", "李四");
            iDatabase.ListRightPop("listKey2");

            //写
            iDatabase.ListLeftPush("listKey1", "listValue1");
            iDatabase.ListLeftPush("listKey1", "listValue2");
            iDatabase.ListLeftPush("listKey1", "listValue3");

            iDatabase.ListLeftPush("listKey2", "listValue1");
            iDatabase.ListLeftPush("listKey2", "listValue2");
            iDatabase.ListLeftPush("listKey2", "listValue3");
            iDatabase.ListLeftPush("listKey2", "listValue3");

            iDatabase.ListLeftPush("listKey3", "listValue1");
            iDatabase.ListLeftPush("listKey3", "listValue2");
            iDatabase.ListLeftPush("listKey3", "listValue3");
            //读:采用FIFO
            RedisValue[] redisValues = iDatabase.ListRange("listKey3");


            //删除
            //出队一个：删除一个
            var redisValue = iDatabase.ListRightPop("listKey2");
            string val = redisValue;

            //从listKey2下List中  删除所有值为listValue3的数据
            long deleteCount = iDatabase.ListRemove("listKey2", "listValue3");

            iDatabase.KeyDelete("listKey1");
            iDatabase.KeyDelete("listKey2");
            iDatabase.KeyDelete("listKey3");

        }
        #endregion

        #region Set无序集合
        private void SetOperaton()
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


            /*
             * Set（集合）:没有重复的值，添加重复值只有一个值。
             它是string类型的无序集合。set是通过hash table实现的，添加，删除和查找,对集合我们可以取并集，交集，差集
             */
            IDatabase iDatabase = GetDatabase();
            iDatabase.SetAdd("setKey1", "setValue1");
            iDatabase.SetAdd("setKey1", "setValue2");
            iDatabase.SetAdd("setKey1", "setValue3");
            iDatabase.SetAdd("setKey1", "setValue3");
            iDatabase.SetAdd("setKey1", "setValue3");

            iDatabase.SetAdd("setKey2", "setValue1");
            iDatabase.SetAdd("setKey2", "setValue2");
            iDatabase.SetAdd("setKey2", "setValue3");


            iDatabase.SetAdd("setKey3", "setValue1");
            iDatabase.SetAdd("setKey3", "setValue2");
            iDatabase.SetAdd("setKey3", "setValue3");

            //读
            RedisValue[] redisValues = iDatabase.SetMembers("setKey2");

            //删
            //LIFO 后进先出
            var setVal = iDatabase.SetPop("setKey2");
            iDatabase.SetRemove("setKey2", "setValue1");

            //集合运算
            RedisValue[] inter = iDatabase.SetCombine(SetOperation.Intersect, "setKey1", "setKey2");
            RedisValue[] union = iDatabase.SetCombine(SetOperation.Union, "setKey1", "setKey2");
            RedisValue[] dif1 = iDatabase.SetCombine(SetOperation.Difference, "setKey1", "setKey2");


            iDatabase.KeyDelete("setKey1");
            iDatabase.KeyDelete("setKey2");
            iDatabase.KeyDelete("setKey3");
        }
        #endregion

        #region  SortedSet 有序集合
        private void SortedSetOperaton()
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

            /*
             sorted set 是set的一个升级版本，它在set的基础上增加了一个顺序的属性，这一属性在添加修改.元素的时候可以指定，
             * 每次指定后，zset(表示有序集合)会自动重新按新的值调整顺序。可以理解为有列的表，一列存 value,一列存顺序。操作中key理解为zset的名字.
             */


            //写
            int i = 0;
            IDatabase iDatabase = GetDatabase();

            iDatabase.KeyDelete("sortedSetKey1");
            iDatabase.KeyDelete("sortedSetKey2");
            iDatabase.KeyDelete("sortedSetKey3");

            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue1", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue2", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue3", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue11", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue12", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue13", ++i);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue12", 100);
            iDatabase.SortedSetAdd("sortedSetKey1", "sortedSetValue13", ++i);

            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue1", ++i);
            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue2", ++i);
            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue3", ++i);
            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue21", ++i);
            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue22", ++i);
            iDatabase.SortedSetAdd("sortedSetKey2", "sortedSetValue23", ++i);

            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue1", ++i);
            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue2", ++i);
            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue3", ++i);
            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue31", Utility.GetTimeStamp());
            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue32", Utility.GetTimeStamp());
            iDatabase.SortedSetAdd("sortedSetKey3", "sortedSetValue33", Utility.GetTimeStamp());

            //读
            RedisValue[] redisValues = iDatabase.SortedSetRangeByRank("sortedSetKey1");

            //倒叙取：取分数最大的。
            RedisValue[] redisValuess = iDatabase.SortedSetRangeByRank("sortedSetKey3", 0, 0, order: Order.Descending);
            string maxRedisValue = redisValuess[0];
            //包含分数
            SortedSetEntry[] sortedSetEntries = iDatabase.SortedSetRangeByRankWithScores("sortedSetKey3", 0, 0, order: Order.Descending);
            string maxValue = sortedSetEntries[0].Element;
            double maxScore = sortedSetEntries[0].Score;
            //集合运算
            var l1 = iDatabase.SortedSetCombineAndStore(SetOperation.Intersect, "sortedSetKey4", "sortedSetKey1", "sortedSetKey2");
            var l2 = iDatabase.SortedSetCombineAndStore(SetOperation.Union, "sortedSetKey5", "sortedSetKey1", "sortedSetKey2");
            //只支持Intersect、Union 其他抛异常
            //var l3 = iDatabase.SortedSetCombineAndStore(SetOperation.Difference, "sortedSetKey6", "sortedSetKey1", "sortedSetKey2");

            //删
            iDatabase.SortedSetRemove("sortedSetKey1", "sortedSetValue13");

            iDatabase.KeyDelete("sortedSetKey1");
            iDatabase.KeyDelete("sortedSetKey2");
            iDatabase.KeyDelete("sortedSetKey3");
            iDatabase.KeyDelete("sortedSetKey4");
            iDatabase.KeyDelete("sortedSetKey5");
        }
        #endregion

        #region 自增
        private void Increment()
        {
            IDatabase iDatabase = GetDatabase();
            iDatabase.StringIncrement("IncrementKey");
            iDatabase.StringIncrement("IncrementKey", 3);
            iDatabase.StringDecrement("IncrementKey");


        }
        #endregion

        #region Expiry
        /// <summary>
        /// key 到期redis会删除
        /// </summary>
        private void ExpiryKey()
        {
            //string 添加的key的时候可以直接添加过期时间，其他设置key的到期时间
            IDatabase iDatabase = GetDatabase();
            iDatabase.StringSet("ExpiryKey", "ExpiryKeyValue", TimeSpan.FromSeconds(20));


            iDatabase.HashSet("hashKey1", "hashField1", "张三");
            iDatabase.KeyExpire("hashKey1", TimeSpan.FromSeconds(20));


            iDatabase.ListLeftPush("listKey1", "listValue1");
            iDatabase.KeyExpire("listKey1", TimeSpan.FromSeconds(20));

            iDatabase.SetAdd("setKey1", "setValue1");
            iDatabase.KeyExpire("setKey1", TimeSpan.FromSeconds(20));
        }
        #endregion

        #region RedisTransaction 事务 
        private void Transaction()
        {
            IDatabase db = GetDatabase();
            var tran = db.CreateTransaction();
            //指定watch 的key 和值，如果相等则执行事务，否则不执行。
            //key 必须存在，否则不执行事务。
            //  var cond = tran.AddCondition(true ? Condition.StringEqual(key2, expected) : Condition.StringNotEqual(key2, expected));
            if (!db.KeyExists("lockeStrKey"))
            {
                db.StringSet("lockeStrKey", "lockeStrValue");
            }
            tran.AddCondition(Condition.StringEqual("lockeStrKey", "lockeStrValue"));
            var incr = tran.StringIncrementAsync("stringIncrementKey");
            var exec = tran.ExecuteAsync();
        }
        #endregion

    }
}
