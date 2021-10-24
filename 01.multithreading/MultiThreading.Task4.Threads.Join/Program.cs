/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            const int value = 10;
            Thread thread = new Thread(ThreedAndJoin);

            thread.Start(value);
            thread.Join();

            Console.WriteLine();

            ThreadPool.QueueUserWorkItem(ThredPoolAndSemaphore, value);

            Console.ReadLine();
        }

        static void ThreedAndJoin(object value)
		{
            int currentValue = (int)value;

            if (currentValue == 0) return;

            currentValue--;
            Console.Write($"{currentValue} ");

            Thread thread = new Thread(ThreedAndJoin);
            thread.Start(currentValue);
            thread.Join();
        }

        static void ThredPoolAndSemaphore(object value)
		{
            semaphore.WaitOne();

            int currentValue = (int)value;

            if (currentValue == 0) return;

            currentValue--;
            Console.Write($"{currentValue} ");

            ThreadPool.QueueUserWorkItem(ThredPoolAndSemaphore, currentValue);

            semaphore.Release();
        }
    }
}
