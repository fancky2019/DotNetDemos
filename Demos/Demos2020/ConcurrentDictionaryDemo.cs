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
         * m_budget（固定值） = 初始容量（31） / （处理器个数-1）;
         * tables.m_buckets.Length>100 或tables.m_countPerLock[lockNo] > m_budget
         * 
         * 扩容大小：之前容量2倍。新增node初始对象。
         * 
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
