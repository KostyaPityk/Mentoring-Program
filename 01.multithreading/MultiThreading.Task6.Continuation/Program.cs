/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            // A
            Task baseTaskA = Task.Run(() =>
            {
                Console.WriteLine("Base task A");
            });

            Task taskA = baseTaskA.ContinueWith((task) =>
            {
                Console.WriteLine("Task A");
            });

            taskA.Wait();
            Console.WriteLine();

           // B
           Task baseTaskB = Task.Run(() =>
            {
                Console.WriteLine("Base task B");
                throw new Exception();
            });

            Task taskB = baseTaskB.ContinueWith((task) =>
            {
                Console.WriteLine("Task B");
                Console.WriteLine(task.Exception.Message);
				
            }, TaskContinuationOptions.OnlyOnFaulted);

            taskB.Wait();
            Console.WriteLine();

            // C
            Task baseTaskC = Task.Run(() =>
            {
                Console.WriteLine("Base task C");
                Console.WriteLine("Current thread " + Thread.CurrentThread.ManagedThreadId);
                throw new Exception();
            });

            Task taskC = baseTaskC.ContinueWith((task) =>
            {
                Console.WriteLine("Task B");
                Console.WriteLine(task.Exception.Message);
                Console.WriteLine("Continue task thread " + Thread.CurrentThread.ManagedThreadId);
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            taskC.Wait();
            Console.WriteLine();

            // D
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Task baseTaskD = Task.Run(() =>
            {
                Console.WriteLine("Base task D");
               
            }, tokenSource.Token);

            Task taskD = baseTaskD.ContinueWith((task) =>
            {
                Console.WriteLine("Task D");
                Console.WriteLine("ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("IsThreadPoolThread: " + Thread.CurrentThread.IsThreadPoolThread);
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            tokenSource.Cancel();
            tokenSource.Dispose();

            Console.ReadLine();
        }
    }
}
