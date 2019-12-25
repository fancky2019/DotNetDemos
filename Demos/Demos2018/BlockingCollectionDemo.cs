using Demos.Demos2018.Model;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2018
{
    class BlockingCollectionDemo
    {
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
