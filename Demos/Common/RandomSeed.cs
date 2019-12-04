using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Common
{
    /// <summary>
    ///Random 不设置seed 同一时刻会产生大量重复值
    /// </summary>
    public class RandomSeed
    {
        /// <summary>
        /// 基本上没有重复的  200W随机27s
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeedByGUID()
        {
            //s实例化一个Guid类
            Guid guid = Guid.NewGuid();
            int key1 = guid.GetHashCode();
            // 摘要:获取表示此实例的日期和时间的计时周期数。
            // 返回结果: 表示此实例的日期和时间的计时周期数。该值介于 DateTime.MinValue.Ticks 和 DateTime.MaxValue.Ticks
            // 之间。
            int key2 = unchecked((int)DateTime.Now.Ticks);
            int seed = unchecked(key1 * key2);
            return seed;
        }

        /// <summary>
        /// 基本上没有重复的  200W随机35s
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static int GetRandom(int minValue, int maxValue)
        {
            // Random 不设置seed 同一时刻会产生大量重复值
            // 使用指定的种子值初始化 System.Random 类的新实例。
            //Seed:用来计算伪随机数序列起始值的数字。如果指定的是负数，则使用其绝对值。
            //这就保证了rand的不同
            int seed = GetRandomSeedByGUID();
            //int seed = GetRandomSeed();
            Random random = new Random(seed);
            return random.Next(minValue, maxValue);
        }
    }
}
