using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    /// <summary>
    /// SpinLockDemo 在未获得锁（Enter(ref lockTaken);）之前一直保持自旋。
    /// 不浪费CPU，不支持锁重入。
    /// </summary>
    public class SpinLockDemo
    {
        public void Test()
        {
            //SpinLockSample1();
            //SpinLockSample2();
            //SpinLockSample3();
            Fun1();
        }

        // Demonstrates:
        //      Default SpinLock construction ()
        //      SpinLock.Enter(ref bool)
        //      SpinLock.Exit()
         void SpinLockSample1()
        {
            SpinLock sl = new SpinLock();

            StringBuilder sb = new StringBuilder();

            // Action taken by each parallel job.
            // Append to the StringBuilder 10000 times, protecting
            // access to sb with a SpinLock.
            Action action = () =>
            {
                bool lockTaken = false;
                for (int i = 0; i < 10000; i++)
                {
                    lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        //Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
                        //做耗时任务，A线程未调用Exit();B线程会一直自旋，直到A线程释放锁。
                        Thread.Sleep(10 * 1000);
                        sb.Append((i % 10).ToString());
                     
                    }
                    finally
                    {
                        // Only give up the lock if you actually acquired it
                        if (lockTaken) sl.Exit();
                    }
                }
            };

            // Invoke 3 concurrent instances of the action above
            Parallel.Invoke(action, action, action);


            // Check/Show the results
            Console.WriteLine("sb.Length = {0} (should be 30000)", sb.Length);
            Console.WriteLine("number of occurrences of '5' in sb: {0} (should be 3000)",
                sb.ToString().Where(c => (c == '5')).Count());
        }

        // Demonstrates:
        //      Default SpinLock constructor (tracking thread owner)
        //      SpinLock.Enter(ref bool)
        //      SpinLock.Exit() throwing exception
        //      SpinLock.IsHeld
        //      SpinLock.IsHeldByCurrentThread
        //      SpinLock.IsThreadOwnerTrackingEnabled
         void SpinLockSample2()
        {
            // Instantiate a SpinLock
            SpinLock sl = new SpinLock();

            // These MRESs help to sequence the two jobs below
            ManualResetEventSlim mre1 = new ManualResetEventSlim(false);
            ManualResetEventSlim mre2 = new ManualResetEventSlim(false);
            bool lockTaken = false;

            Task taskA = Task.Factory.StartNew(() =>
            {
                try
                {
                    sl.Enter(ref lockTaken);
                    Console.WriteLine("Task A: entered SpinLock");
                    mre1.Set(); // Signal Task B to commence with its logic

                    // Wait for Task B to complete its logic
                    // (Normally, you would not want to perform such a potentially
                    // heavyweight operation while holding a SpinLock, but we do it
                    // here to more effectively show off SpinLock properties in
                    // taskB.)
                    mre2.Wait();
                }
                finally
                {
                    if (lockTaken) sl.Exit();
                }
            });

            Task taskB = Task.Factory.StartNew(() =>
            {
                mre1.Wait(); // wait for Task A to signal me
                Console.WriteLine("Task B: sl.IsHeld = {0} (should be true)", sl.IsHeld);
                Console.WriteLine("Task B: sl.IsHeldByCurrentThread = {0} (should be false)", sl.IsHeldByCurrentThread);
                Console.WriteLine("Task B: sl.IsThreadOwnerTrackingEnabled = {0} (should be true)", sl.IsThreadOwnerTrackingEnabled);

                try
                {
                    sl.Exit();
                    Console.WriteLine("Task B: Released sl, should not have been able to!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task B: sl.Exit resulted in exception, as expected: {0}", e.Message);
                }

                mre2.Set(); // Signal Task A to exit the SpinLock
            });

            // Wait for task completion and clean up
            Task.WaitAll(taskA, taskB);
            mre1.Dispose();
            mre2.Dispose();
        }

        // Demonstrates:
        //      SpinLock constructor(false) -- thread ownership not tracked
         void SpinLockSample3()
        {
            // Create SpinLock that does not track ownership/threadIDs
            SpinLock sl = new SpinLock(false);

            // Used to synchronize with the Task below
            ManualResetEventSlim mres = new ManualResetEventSlim(false);

            // We will verify that the Task below runs on a separate thread
            Console.WriteLine("main thread id = {0}", Thread.CurrentThread.ManagedThreadId);

            // Now enter the SpinLock.  Ordinarily, you would not want to spend so
            // much time holding a SpinLock, but we do it here for the purpose of 
            // demonstrating that a non-ownership-tracking SpinLock can be exited 
            // by a different thread than that which was used to enter it.
            bool lockTaken = false;
            sl.Enter(ref lockTaken);

            // Create a separate Task from which to Exit() the SpinLock
            Task worker = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("worker task thread id = {0} (should be different than main thread id)",
                    Thread.CurrentThread.ManagedThreadId);

                // Now exit the SpinLock
                try
                {
                    sl.Exit();
                    Console.WriteLine("worker task: successfully exited SpinLock, as expected");
                }
                catch (Exception e)
                {
                    Console.WriteLine("worker task: unexpected failure in exiting SpinLock: {0}", e.Message);
                }

                // Notify main thread to continue
                mres.Set();
            });

            // Do this instead of worker.Wait(), because worker.Wait() could inline the worker Task,
            // causing it to be run on the same thread.  The purpose of this example is to show that
            // a different thread can exit the SpinLock created (without thread tracking) on your thread.
            mres.Wait();

            // now Wait() on worker and clean up
            worker.Wait();
            mres.Dispose();
        }

        private void Fun1()
        {
            SpinLock sl = new SpinLock(false);

       
            bool lockTaken = false;
 
            Task.Run(() =>
            {
                //获得锁
                sl.Enter(ref lockTaken);
                Console.WriteLine("task1 exit.");
                Thread.Sleep(20000);
                //释放锁
                //sl.Exit();
            });

            bool lockTaken1 = false;
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                //一直在此自旋等待。直到task120s后释放锁
                sl.Enter(ref lockTaken1);
                int m = 0;
                //释放锁
                sl.Exit();
            });
        }

    }
}
