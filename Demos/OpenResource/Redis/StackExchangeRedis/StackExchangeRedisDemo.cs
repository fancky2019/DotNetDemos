using MbUnit.Framework;
using Redlock.CSharp;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.Redis.StackExchangeRedis
{
    public class StackExchangeRedisDemo
    {
        //StackExchange.Redis
        //github:https://github.com/ServiceStack/ServiceStack.Redis
        public void Test()
        {
            DistributedLock();
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        private void DistributedLock()
        {
            //github:https://github.com/kidfashion/redlock-cs
            //添加Dll:Redlock.CSharp，NuGet安装StackExchange.Redis
            //或者添加Dll:Redlock.CSharp、StackExchange.Redis，其他依赖dll参照nuget。
            TestWhenLockedAnotherLockRequestIsRejected();
            // TestThatSequenceLockedUnlockedAndLockedAgainIsSuccessfull();
        }
        const string key = "key";
        public void TestWhenLockedAnotherLockRequestIsRejected()
        {
            ////单服务器
            //var dlm = new Redlock.CSharp.Redlock(ConnectionMultiplexer.Connect("127.0.0.1:6379,password=fancky123456,DefaultDatabase=0"));

            //Lock lockObject;
            //Lock newLockObject;
            //var locked = dlm.Lock(key, new TimeSpan(0, 0, 10), out lockObject);
            //Assert.IsTrue(locked, "Unable to get lock");
            //locked = dlm.Lock(key, new TimeSpan(0, 0, 10), out newLockObject);
            //Assert.IsFalse(locked, "lock taken, it shouldn't be possible");
            //dlm.Unlock(lockObject);


            var dlm = new Redlock.CSharp.Redlock(ConnectionMultiplexer.Connect("127.0.0.1:6379,password=fancky123456,DefaultDatabase=0"));

            Lock lockObject;
            Lock newLockObject;
            var locked = dlm.Lock(key, new TimeSpan(0, 0, 10), out lockObject);
            Console.WriteLine(locked);
         
            locked = dlm.Lock(key, new TimeSpan(0, 0, 3), out newLockObject);
            //  Assert.IsFalse(locked, "lock taken, it shouldn't be possible");
            Console.WriteLine(locked);
            Thread.Sleep(3);
            //3s秒后Key过期释放锁
            //RedLock 内部调用： redis.GetDatabase().StringSet(resource, val, ttl, When.NotExists);
            locked = dlm.Lock(key, new TimeSpan(0, 0, 3), out newLockObject);
            dlm.Unlock(lockObject);
        }

        public void TestThatSequenceLockedUnlockedAndLockedAgainIsSuccessfull()
        {
            //分布式集群
            var dlm = new Redlock.CSharp.Redlock(ConnectionMultiplexer.Connect("127.0.0.1:6379"), ConnectionMultiplexer.Connect("127.0.0.1:6380"), ConnectionMultiplexer.Connect("127.0.0.1:6381"));
            Lock lockObject = null;
            Lock newLockObject;

            var locked = dlm.Lock(key, new TimeSpan(0, 0, 10), out lockObject);
            Assert.IsTrue(locked, "Unable to get lock");
            dlm.Unlock(lockObject);
            locked = dlm.Lock(key, new TimeSpan(0, 0, 10), out newLockObject);
            Assert.IsTrue(locked, "Unable to get lock");

            dlm.Unlock(newLockObject);


        }
    }
}
