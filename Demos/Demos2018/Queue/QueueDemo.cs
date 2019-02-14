using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2018.Queue
{
    class QueueDemo
    {
        public void  Test()
        {

            //并发队列：参照Demos2018下 SynchronizationDemo下的ProducerConsumer
            ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
            //阻塞队列 ：参照Demos2018下的BlockingCollectionDemo
            BlockingCollection<int> blockingCollection = new BlockingCollection<int>();
        }
    }
}
