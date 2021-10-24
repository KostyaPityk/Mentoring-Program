/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
			Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
			Console.WriteLine("First Task – creates an array of 10 random integer.");
			Console.WriteLine("Second Task – multiplies this array with another random integer.");
			Console.WriteLine("Third Task – sorts this array by ascending.");
			Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
			Console.WriteLine();

			const int arrayCount = 10;
			Random random = new Random();

			Task<int[]> first = Task.Run(() =>
			{
				int[] array = new int[arrayCount];

				for (int i = 0; i < arrayCount; i++)
				{
					array[i] = random.Next(0, 20);
				}

				Print(1, array);

				return array;
			});


			Task<int[]> second = first.ContinueWith(parentTask =>
			{
				int[] array = parentTask.Result;
				int multiplier = random.Next(1, 10);

				for (int i = 0; i < arrayCount; i++)
				{
					array[i] *= multiplier;
				}

				Print(2, array);

				return array;
			});

			Task<int[]> third = second.ContinueWith(parentTask =>
			{
				int[] array = parentTask.Result.OrderBy(x => x).ToArray();

				Print(3, array);

				return array;
			});

			Task fourth = third.ContinueWith(parentTask =>
			{
				double averageValue = parentTask.Result.Average();

				Console.WriteLine($"Task id: 4; avg: {averageValue}");
			});

			Console.ReadLine();
		}

		static void Print(int taskId, int[] array)
		{
			Console.WriteLine($"Task id: {taskId}");

			foreach (int item in array)
				Console.Write($"{item} ");

			Console.WriteLine();
		}
	}
}
