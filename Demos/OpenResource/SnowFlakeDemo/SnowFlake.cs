﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.OpenResource.SnowFlakeDemo
{
    /**
 * Twitter_Snowflake<br>
 * SnowFlake的结构如下(每部分用-分开):<br>
 * 0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000 <br>
 * 1位标识，由于long基本类型在Java中是带符号的，最高位是符号位，正数是0，负数是1，所以id一般是正数，最高位是0<br>
 * 41位时间截(毫秒级)，注意，41位时间截不是存储当前时间的时间截，而是存储时间截的差值（当前时间截 - 开始时间截)
 * 得到的值），这里的的开始时间截，一般是我们的id生成器开始使用的时间，由我们程序来指定的（如下下面程序IdWorker类的startTime属性）。41位的时间截，可以使用69年，年T = (1L << 41) / (1000L * 60 * 60 * 24 * 365) = 69<br>
 * 10位的数据机器位，可以部署在1024个节点，包括5位datacenterId和5位workerId<br>
 * 12位序列，毫秒内的计数，12位的计数顺序号支持每个节点每毫秒(同一机器，同一时间截)产生4096个ID序号<br>
 * 加起来刚好64位，为一个Long型。<br>
 * SnowFlake的优点是，整体上按照时间自增排序，并且整个分布式系统内不会产生ID碰撞(由数据中心ID和机器ID作区分)，并且效率较高，经测试，SnowFlake每秒能够产生26万ID左右。
 */
    public class SnowFlake
    {

        // ==============================Fields===========================================
        /// <summary>
        ///  开始时间截 (2015-01-01) 
        ///  生产环境做成可配置
        /// </summary>
        private static readonly long _twepoch;


        /*C#右操作数必须是int,而不是long.使用long作为移位的位数是没有意义的,
            因为C#中的整数类型永远不会超过64位.


          美团的Leaf:将DataCenter和WorkerID合并在一起10字节
         */



        /// <summary>
        ///机器id所占的位数
        /// </summary>
        private const int _workerIdBits = 5;

        /// <summary>
        ///数据标识id所占的位数
        /// </summary>
        private const int _datacenterIdBits = 5;

        /// <summary>
        ///  支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数) 
        /// </summary>
        private const long _maxWorkerId = -1L ^ (-1L << _workerIdBits);

        /// <summary>
        ///支持的最大数据标识id，结果是31 
        /// </summary>
        private const long _maxDatacenterId = -1L ^ (-1L << _datacenterIdBits);

        /// <summary>
        ///序列在id中占的位数 
        /// </summary>
        private const int _sequenceBits = 12;

        /// <summary>
        ///机器ID向左移12位
        /// </summary>
        private const int _workerIdShift = _sequenceBits;

        /// <summary>
        /// 数据标识id向左移17位(12+5) 
        /// </summary>
        private const int _datacenterIdShift = _sequenceBits + _workerIdBits;

        /// <summary>
        ///时间截向左移22位(5+5+12) 
        /// </summary>
        private const int _timestampLeftShift = _sequenceBits + _workerIdBits + _datacenterIdBits;

        /// <summary>
        ///生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095) 
        /// </summary>
        private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);

        /// <summary>
        /// 工作机器ID(0~31) 
        /// </summary>
        private long _workerId;

        /// <summary>
        /// 数据中心ID(0~31) 
        /// </summary>
        private long _datacenterId;

        /// <summary>
        ///毫秒内序列(0~4095)
        /// </summary>
        private long _sequence = 0L;

        /// <summary>
        /// 上次生成ID的时间截 
        /// </summary>
        private long _lastTimestamp = -1L;

        private object _lockObj = new object();

        static SnowFlake()
        {
            //_twepoch = (DateTime.Parse("2020-01-01").Ticks - DateTime.Parse("1970-01-01").Ticks) / 10000; 

            _twepoch = DateTime.Parse("2020-01-01").Ticks / 10000;
        }
        //==============================Constructors=====================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workerId">workerId 工作ID(0~31)</param>
        /// <param name="datacenterId">datacenterId 数据中心ID(0~31)</param>
        public SnowFlake(long workerId, long datacenterId)
        {
            if (workerId > _maxWorkerId || workerId < 0)
            {
                throw new Exception($"worker Id can't be greater than {_maxWorkerId} or less than 0");
            }
            if (datacenterId > _maxDatacenterId || datacenterId < 0)
            {
                throw new Exception($"datacenter Id can't be greater than {_maxDatacenterId} or less than 0");
            }
            this._workerId = workerId;
            this._datacenterId = datacenterId;
        }

        // ==============================Methods==========================================
        /**
         * 获得下一个ID (该方法是线程安全的)
         * @return SnowflakeId
         */
        public long NextId()
        {

            lock (_lockObj)
            {
                long timestamp = TimeGen();
                //如果当前时间小于上一次ID生成的时间戳，说明系统时钟回退过这个时候应当抛出异常
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception($"Clock moved backwards.  Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
                }

                /*
                 * 2^12:同一个中心的机器1ms最多生成4096个
                 */

                //如果是同一时间生成的，则进行毫秒内序列
                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    //毫秒内序列溢出
                    if (_sequence == 0)
                    {
                        //阻塞到下一个毫秒,获得新的时间戳
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                //时间戳改变，毫秒内序列重置
                else
                {
                    _sequence = 0L;
                }

                //上次生成ID的时间截
                _lastTimestamp = timestamp;

                var r1 = (timestamp - _twepoch);
                var r2 = r1 << _timestampLeftShift;
                //1110001010110011000001011010100000100000000010
                //1110 0010 1011 0011 0000 0101 1010 1000 0010 0000 0000 10
                var str = System.Convert.ToString(r1, 2);
                //移位并通过或运算拼到一起组成64位的ID
                return ((timestamp - _twepoch) << _timestampLeftShift) //
                        | (_datacenterId << _datacenterIdShift) //
                        | (_workerId << _workerIdShift) //
                        | _sequence;
            }

        }

        /// <summary>
        /// 阻塞到下一个毫秒，直到获得新的时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns>当前时间戳</returns>
        protected long TilNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        ///  返回以毫秒为单位的当前时间
        /// </summary>
        /// <returns>当前时间(毫秒)</returns>
        protected long TimeGen()
        {
            //return DateTime.Now.Ticks / 10000;
            return DateTime.Now.Ticks / 10000;

        }

        //==============================Test=============================================

        public long Test()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();



            //for (int i = 0; i < 100000; i++)
            //{
            //    //3111653662208819200
            //    //3111654027293622272
            //    //3112084979752894464
            //    long id = idWorker.NextId();
            //var str = System.Convert.ToString(id, 2);
            //    //Console.WriteLine(id);
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"ns:{stopwatch.ElapsedTicks * GetNanosecPerTick()}");
            //Console.WriteLine($"ms:{stopwatch.ElapsedMilliseconds}");
            long id = this.NextId();
            return id;

        }

        /// <summary>
        /// 获取当前系统一个时钟周期多少纳秒
        /// </summary>
        /// <returns></returns>
        public long GetNanosecPerTick()
        {
            //1秒(s) =100厘秒(cs)= 1000 毫秒(ms) = 1,000,000 微秒(μs) = 1,000,000,000 纳秒(ns) = 1,000,000,000,000 皮秒(ps)
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            return nanosecPerTick;
        }
    }
}
