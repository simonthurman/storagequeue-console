// See https://aka.ms/new-console-template for more information
using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
//using Azure.Identity;
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage

namespace storagequeue
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set the connection string and queue name
            string connectionString = "<connection_string>";
            string queueName = "myqueue";

            // Create a QueueClient which will be used to create and manipulate the queue
            var myqclient = new QueueClient(connectionString, queueName);

            // Create the queue
            await myqclient.CreateIfNotExistsAsync();
            
            // Send a message to the queue
            await myqclient.SendMessageAsync("Hello, World!");

            // Receive the message from the queue
            QueueMessage[] messages = await myqclient.ReceiveMessagesAsync();

            // Process the message
            foreach (QueueMessage message in messages)
            {
                Console.WriteLine($"Message: {message.MessageText}");
                Console.WriteLine($"MessageId: {message.MessageId}");
                Console.WriteLine($"PopReceipt: {message.PopReceipt}");
                Console.WriteLine($"ExpiresOn: {message.ExpiresOn}");
                Console.WriteLine($"TimeNextVisible: {message.NextVisibleOn}");
            }

            // wait 35 seconds
            await Task.Delay(35000);

            // Update message content
            await myqclient.UpdateMessageAsync(messages[0].MessageId, messages[0].PopReceipt, "Updated contents", TimeSpan.FromSeconds(60.0));

            // Delete the message from the queue
            await myqclient.DeleteMessageAsync(messages[0].MessageId, messages[0].PopReceipt);

            // Delete the queue
            await myqclient.DeleteIfExistsAsync();
        }
    }
}