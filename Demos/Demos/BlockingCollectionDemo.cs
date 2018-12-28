using Demos.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos
{
    class BlockingCollectionDemo
    {
        static BlockingCollection<Product> blockingCollection = null;
        static BlockingCollectionDemo()
        {
            blockingCollection = new BlockingCollection<Product>();
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
                    while (!blockingCollection.IsAddingCompleted)
                    {
                        int next = rd.Next(1, 10000);
                        blockingCollection.Add(new Product() {
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
                foreach (Product product in blockingCollection.GetConsumingEnumerable())
                {
                    i++;
                    if(i==100)
                    {
                        blockingCollection.CompleteAdding();
                    }
                    Console.WriteLine($"{product.ID}:{product.ProductName} ");
                }
            });
        }
    }
}
