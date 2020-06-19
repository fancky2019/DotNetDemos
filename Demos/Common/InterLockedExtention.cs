using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Common
{
    /// <summary>
    /// 采用CAS思想实现轻量锁。
    /// 测试发现：Debug下Monitor的性能好
    ///           Release下InterLockedExtention拓展好。
    ///
    /// 不能设计成静态方法，不然所有要加锁地方竞争一把锁。项目中实际改成实例方法。
    /// </summary>
    public class InterLockedExtention
    {
        /// <summary>
        /// 1:锁被占用，0：未占用
        /// </summary>
        private volatile int _lock = 0;

        /// <summary>
        /// 获取锁
        /// </summary>
        /// <returns></returns>
        public bool Acquire()
        {
            //尝试获取锁
            return Interlocked.CompareExchange(ref _lock, 1, 0) == 0;

        }

        /// <summary>
        /// 自旋直到获得锁，此种可以用FCL的SpinLock
        /// 自旋会吃掉一个核的CPU。SpinLock不会。
        /// </summary>
        /// <returns></returns>
        public bool SpinUntilAcquire()
        {
            while (!(Interlocked.CompareExchange(ref _lock, 1, 0) == 0))
            {
                SpinWait spinWait = default;
                spinWait.SpinOnce();
            }
            //尝试获取锁
            return true;

        }

        /// <summary>
        /// 释放锁
        /// </summary>
        public void Release()
        {
            //释放锁
            //Interlocked.CompareExchange(ref _lock, 0, 1);
            //其实不用比较，此方法只有在获取锁的块内调用
            Interlocked.Exchange(ref _lock, 0);

        }



    }
}
