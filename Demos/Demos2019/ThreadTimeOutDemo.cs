using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    class ThreadTimeOutDemo
    {
        public async void Test()
        {
            //TaskTimeOut();
            //ThreadTimeOut();








            int orderId = 10;
            bool success = false;
            var task = TimeOutService(() =>
              {
                  Console.WriteLine($"开始更新订单{orderId}！");
                  Thread.Sleep(7000);
                  success = true;

                  return true;
              },
               (p) =>
               {
                   Console.WriteLine($"更新订单{orderId}超时，失败！");
                   success = false;
               }, orderId);

   
            var re = await task;
















            //cancellationtokensource source = new cancellationtokensource();

            //int timeoutactionparameter = 10;
            //source.token.register((timeoutactionparameter) =>
            //{

            //}, timeoutactionparameter);
            //task<int> task = task.run(() =>
            //{
            //    thread.sleep(5000);
            //    return 1;
            //}, source.token).timeoutafter<int>(timespan.frommilliseconds(3000), source);
            //if (task.iscanceled)
            //{
            //    //超时被取消了
            //}
            //else
            //{
            //    int re = task.result;
            //}

        }

        private void TaskTimeOut()
        {
            //   CancellationTokenSource source = null;
            try
            {
                int timeOut = 3000;
                CancellationTokenSource source = new CancellationTokenSource();//实际操作可为全局变量，避免每次都创建
                CancellationToken token = source.Token;
                int orderId = 10;
                token.Register((p) =>
                {
                    Console.WriteLine($"Oder:{orderId} deal failed!");
                }, orderId);
                Task.Run(() =>
                  {
                      try
                      {
                          Thread.Sleep(5000);//DoWork()
                                             //判断是否取消
                          if (token.IsCancellationRequested)
                          {
                              //rollback
                          }
                          //如果取消了 ，抛异常
                          token.ThrowIfCancellationRequested();
                      }
                      catch (Exception e)
                      {
                          //线程内的异常，不会被外面的线程捕获。
                          throw e;
                      }
                      finally
                      {
                          source.Dispose();//soour 不能在CancelAfter之前释放资源。否则因对象被释放而无法取消。
                      }
                  }, token);
                //设置线程执行3秒超时取消
                source.CancelAfter(timeOut);


                //Thread.Sleep(3000);
                //source.Cancel();



            }
            //全局异常无法捕获子线程内的异常
            catch (AggregateException ex)
            {

            }
            catch (Exception ex)
            {

            }



        }

        private Task<ResponseMessage<TResult>> TimeOutService<TResult>(Func<TResult> service, Action<object> timeOutAction, object timeOutActionParameter)
        {
            try
            {
                if (service == null || timeOutAction == null || timeOutActionParameter == null)
                {
                    throw new Exception("parameter must't be null!");
                }
                int timeOut = 3000;//配置文件设置
                CancellationTokenSource source = new CancellationTokenSource();//实际操作可为全局变量，避免每次都创建

                CancellationToken token = source.Token;
                token.Register(timeOutAction, timeOutActionParameter);
                Task<ResponseMessage<TResult>> task = Task.Run<ResponseMessage<TResult>>(() =>
                  {
                      ResponseMessage<TResult> responseMessage = new ResponseMessage<TResult>();
                      try
                      {
                          //根据service的执行返回，判断是否执行成功。
                          var re = service();


                          token.ThrowIfCancellationRequested();
                          ////判断是否取消
                          //if (token.IsCancellationRequested)
                          //{
                          //    //rollback
                          //}
                          //else
                          //{
                          //    return re;
                          //}
                          return responseMessage;
                      }
                      catch (Exception e)
                      {
                          Console.WriteLine(e.Message);
                          responseMessage.Success = false;
                          responseMessage.Message = e.Message;
                          responseMessage.Result= default(TResult);
                          return responseMessage;
                      }
                      finally
                      {
                          source.Dispose();
                      }
                  }, token);
                //设置线程执行3秒超时取消
                source.CancelAfter(timeOut);

                return task;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void ThreadTimeOut()
        {
            int timeOut = 3000;
            bool complete = false;
            object _lockObj = new object();
            Thread thread = new Thread(() =>
             {
                 //Thread.Sleep(5000);
                 int i = 0;
                 while (true)
                 {
                     Console.WriteLine(++i);

                     Thread.Sleep(1000);
                     if (i == 10)
                     {
                         break;
                     }
                 }
                 lock (_lockObj)
                 {
                     complete = true;
                 }
                 //if (Thread.CurrentThread.IsAlive)
                 //{

                 //}

                 //ThreadState threadState = Thread.CurrentThread.ThreadState;
                 //Console.WriteLine(threadState);

             });
            thread.Start();

            //java守护线程 == C#的后台线程
            Thread daemon = new Thread(() =>
             {
                 Thread.Sleep(timeOut);
                 lock (_lockObj)
                 {
                     if (!complete)
                     {
                         thread.Abort();

                         // thread.Join();//调用join之后，将阻止该调用线程，直到thread线程结束。后面的WriteLine将不执行。
                         // Console.WriteLine("Join() Called!");

                     }
                 }

             });

            daemon.Start();


            Thread.Sleep(4000);

            ////主线程中调用
            //thread.Abort();//调用Abort方法线程并不能立即终止，有点延迟，需要阻止当前调用线程一段时间，等待线程终止
            //thread.Join(); //调用join之后，将阻止该调用线程，直到thread线程结束。
            if (thread.IsAlive)//false
            {

            }
            ThreadState threadState = thread.ThreadState;//Aborted
            Console.WriteLine(threadState);//Aborted
        }





    }


    /// <summary>
    /// 
    /// </summary>
    public static class TaskTimeOutExtension
    {
        // 有返回值
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationTokenSource taskCancellationTokenSource = null)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    taskCancellationTokenSource?.Cancel();
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }

        // 无返回值
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }

    class ResponseMessage<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

}
