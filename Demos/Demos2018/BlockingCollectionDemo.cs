using Demos.Demos2018.Model;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class BlockingCollectionDemo
    {
       // 生成的线程能调 CompleteAdding方法，以指示将添加更多的项。 使用者将监视 IsCompleted属性以了解当集合为空，将添加更多的项。

        static BlockingCollection<Product> _blockingCollection = null;
        static BlockingCollectionDemo()
        {
            _blockingCollection = new BlockingCollection<Product>();
        }
        public void Test()
        {
            Consumer();
            Producer();
        }

        private void Consumer()
        {
            Random rd = new Random();
            Task.Run(() =>
                {
                    while (!_blockingCollection.IsAddingCompleted)
                    {
                        int next = rd.Next(1, 10000);
                        _blockingCollection.Add(new Product() {
                            ID= next,
                            ProductName=$"test{next}"
                        });
                        Thread.Sleep(200);
                    }
                });
        }

        private void Producer()
        {
            Task.Run(() =>
            {
                int i = 0;
                foreach (Product product in _blockingCollection.GetConsumingEnumerable())
                {
                    i++;
                    if(i==100)
                    {
                        _blockingCollection.CompleteAdding();
                    }
                    Console.WriteLine($"{product.ID}:{product.ProductName} ");
                }
            });
        }
    }
}
