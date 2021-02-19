// Using Directives
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

//Create a new Program class with two constant string properties named 
// storageConnectionString and messagequeue, and then create an asynchronous Main entry point method
public class Program
{
    private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=asyncstorks;AccountKey=QRmnS0ArVTyu7eSZKrIceLsgJBEKAO1HqLD8oqkJtWAqf+qLoVkrL3NXM0syTsVKF/xcYuM5B7QLB58/Ke96+g==;EndpointSuffix=core.windows.net";
    private const string queueName = "messagequeue";

    public static async Task Main(string[] args)
    {
        // In the Main method, add the following block of code to connect to the storage account and to asynchronously create the queue if it doesn't already exist
        QueueClient client = new QueueClient(storageConnectionString, queueName);        
        await client.CreateAsync();

        // Still in the Main method, add the following block of code to render the Uniform Resource Identifier (URI) of the queue endpoint
        Console.WriteLine($"---Account Metadata---");
        Console.WriteLine($"Account Uri:\t{client.Uri}");

        // Render a header by using the Console.WriteLine static method
        Console.WriteLine($"---Existing Messages---");

        // Add the following block of code to create variables that will be used when retrieving queue messages
        int batchSize = 10;
        TimeSpan visibilityTimeout = TimeSpan.FromSeconds(2.5d);

        // Add the following block of code to retrieve a batch of messages asynchronously from the queue service and iterate over the messages
        Response<QueueMessage[]> messages = await client.ReceiveMessagesAsync(batchSize, visibilityTimeout);
        foreach(QueueMessage message in messages?.Value)
        {
            // Within the foreach block, render the MessageId and MessageText properties of the QueueMessage instance
            Console.WriteLine($"[{message.MessageId}]\t{message.MessageText}");
            // Update the foreach loop's block of code to invoke the DeleteMessageAsync method of the QueueMessage class, passing in the MessageId and PopReceipt properties of the message variable
            //await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        // Render a header by using the Console.WriteLine static method
        Console.WriteLine($"---New Messages---");

        // Add the following block of code to create a new string variable named greeting with a value of Hi, Developer!, 
        //and then invoke the SendMessageAsync method of the QueueClient class by using the greeting variable as a parameter
        string greeting = "Hola, Developer!";
        await client.SendMessageAsync(greeting);

        // Add the following block of code to render the content of the message that you sent
        Console.WriteLine($"Sent Message:\t{greeting}");
    }
}

