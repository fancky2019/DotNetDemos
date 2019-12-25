using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// 使用自旋，避免线程上下文切换
    /// </summary>
    public class SpinWaitDemo
    {
        public void  Test()
        {
            fun();
        }

        private  void  fun()
        {
            SpinWait spinWait = default(SpinWait);

            while(true)
            {
                // spinWait.SpinOnce();
                //将一直自旋等待
               // SpinWait.SpinUntil(() =>false);

                //自旋3秒
                SpinWait.SpinUntil(() => false,3*1000);
                //自旋
                spinWait.SpinOnce();
            }
        }
    }
}
