using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    /// <summary>
    /// 
    /// </summary>
    class ConcurrentDictionaryDemo
    {
        /*
         * hashcode获取：实现IEqualityComparer接口的GetHashCode方法
         * 
         * 
         * 默认数组长度：
         * buckets：31
         * locks=countPerLock：处理器个数-1
         * 
         * buckets:capacity>=(locks=countPerLock=处理器个数-1)
         * Tables(Node[] buckets, object[] locks, int[] countPerLock,
         * 
         * 
         * key不存在或者hash碰撞。新加到m_buckets[bucketNo]位置：
         * 保存数据到node。头部插入node,next指向 tables.m_buckets[bucketNo])
         * 
         * Volatile.Write(ref tables.m_buckets[bucketNo], new Node(key, value, hashCode, tables.m_buckets[bucketNo]));  
         * 
         * 扩容条件：
         *
         *budget=buckets/locks
         * 节点hash碰撞数>100 或tables.m_countPerLock[lockNo] > m_budget。即：锁对象锁的个数超过预算。
         * 
         * 扩容大小：之前容量2倍。新增node初始对象。
         * 
         * 
         * 
         * 
         * hash桶位置bucketNo：hashcode%hash桶大小
         * 锁位置lockNo：bucketNo%lockCount
         * 
         * 
         * 
         * 整个加锁:Monitor Enter整个数组
         * Monitor.Enter(locks[i], ref lockTaken);
         * 释放整个锁：Monitor Exit整个数组
         * 
         */
        public void Test()
        {
            ConcurrentDictionary<int, int> concurrentDictionary = new ConcurrentDictionary<int, int>();
            var re = concurrentDictionary.TryAdd(1, 1);
            //key  存在添加不进去
            re = concurrentDictionary.TryAdd(1, 2);

            if (concurrentDictionary.TryGetValue(1, out int m))
            {
                int n = m;
            }

            //concurrentDictionary.AddOrUpdate()

        }
    }
}
