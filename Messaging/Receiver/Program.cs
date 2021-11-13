using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Receiver
{
	class Program
	{
		const string connectionString = "Endpoint=sb://testmessagingmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=w+AmCBIa4UqRE5m1IziQ45eNZL0VP5xttfrfUaj1WVA=";
		const string queueName = "messaging";
		const string outputDirectory = "D:\\Test\\Output";
		static IQueueClient queueClient;

		static void Main(string[] args)
		{
			ReceiveMessages().Wait();
		}

		static async Task ReceiveMessages()
		{
			ManagementClient managementClient = new ManagementClient(connectionString);

			bool isQueueExist = await managementClient.QueueExistsAsync(queueName);

			if (!isQueueExist)
			{
				await managementClient.CreateQueueAsync(queueName);
			}

			queueClient = new QueueClient(connectionString, queueName, ReceiveMode.ReceiveAndDelete);


			MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions((ExceptionReceivedEventArgs arg) => Task.FromResult(arg.Exception))
			{
				MaxConcurrentCalls = 1,
				AutoComplete = false
			};

			queueClient.RegisterMessageHandler(MessageHandlerAsync, messageHandlerOptions);

			Console.ReadKey();
			await queueClient.CloseAsync();
		}

		static async Task MessageHandlerAsync(Message message, CancellationToken token)
		{
			string fileName = message.Label;
			int filePosition = Convert.ToInt32(message.UserProperties["position"]);
			string filePath = Path.Combine(outputDirectory, fileName);

			using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
			{
				fileStream.Position = filePosition;
				fileStream.Write(message.Body);
			}

			await queueClient.CompleteAsync(message.SystemProperties.LockToken);

		}
	}
}
