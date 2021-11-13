using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System.IO;

namespace Sender
{
	class Program
	{
		const string connectionString = "Endpoint=sb://testmessagingmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=w+AmCBIa4UqRE5m1IziQ45eNZL0VP5xttfrfUaj1WVA=";
		const string queueName = "messaging";
		const string watchDirectory = "D:\\Test\\Input";
		const int maxMessageSize = 256000;

		static IQueueClient queueClient = new QueueClient(connectionString, queueName);

		static void Main(string[] args)
		{
			WatcherAsync().Wait();
		}

		static async Task WatcherAsync()
		{
			FileSystemWatcher directoryWathcer = new FileSystemWatcher(watchDirectory);
			directoryWathcer.Created += SendMessageAsync;
			directoryWathcer.EnableRaisingEvents = true;

			ConsoleKeyInfo inputKey = Console.ReadKey();
			if (inputKey.Key == ConsoleKey.Backspace)
			{
				await queueClient.CloseAsync();
			}
		}


		static async void SendMessageAsync(object sender, FileSystemEventArgs e)
		{
			try
			{
				using (FileStream fileStream = new FileStream(e.FullPath, FileMode.Open))
				{
					while(fileStream.Position != fileStream.Length)
					{
						byte[] messageBody = new byte[maxMessageSize];

						Message message = new Message(messageBody);
						message.Label = e.Name;
						message.UserProperties.Add("position", fileStream.Position);

						var readBytes = fileStream.Read(messageBody, 0, maxMessageSize);

						await queueClient.SendAsync(message);
					}
				}

				Console.WriteLine("Successfully");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception - ${ex.Message}");
			}
		}
	}
}
