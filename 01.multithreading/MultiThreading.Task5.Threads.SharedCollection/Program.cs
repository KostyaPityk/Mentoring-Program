/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace MultiThreading.Task5.Threads.SharedCollection
{
	class Program
	{

		static AutoResetEvent addWaitHandler = new AutoResetEvent(false);
		static AutoResetEvent printWaitHandler = new AutoResetEvent(true);

		static int itemsCount = 10;
		static List<int> collection = new List<int>();

		static void Main(string[] args)
		{
			Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
			Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
			Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
			Console.WriteLine();

			// feel free to add your code
			Task firstTask = Task.Run(Add);
			Task secondTask = Task.Run(Print);

			Console.ReadLine();
		}

		static void Add()
		{
			for (int i = 0; i < itemsCount; i++)
			{
				printWaitHandler.WaitOne();
				collection.Add(i);
				addWaitHandler.Set();
			}
		}

		static void Print()
		{
			for (int i = 0; i < itemsCount; i++)
			{
				addWaitHandler.WaitOne();
				Console.WriteLine(string.Join(", ", collection));
				printWaitHandler.Set();
			}

		}
	}
}
