// using directives for libraries that will be referenced by the application
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

// Program class with three constant string properties named 
// blobServiceEndpoint, storageAccountName, and storageAccountKey, and then create an asynchronous Main entry point method
public class Program
{
    private const string blobServiceEndpoint = "https://mediastorks.blob.core.windows.net/";
    private const string storageAccountName = "mediastorks";
    private const string storageAccountKey = "zVftHyP8xnmVS/sp4xKBM9jaubQ7MiGJP8K9RnkSmohAxXvcdYq29BnL1tbXlQ66IG9eXwQC7BwtoKkCvynJIg==";
    

    public static async Task Main(string[] args)
    {
        // code to connect to the storage account and to retrieve account metadata
        StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);

        AccountInfo info = await serviceClient.GetAccountInfoAsync();

        // block of code to print metadata about the storage account
        await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");
        await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
        await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
        await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

        await EnumerateContainersAsync(serviceClient);

        string existingContainerName = "raster-graphics";
        await EnumerateBlobsAsync(serviceClient, existingContainerName);
    
        string newContainerName = "vector-graphics";
        BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);
    
        string uploadedBlobName = "graph.svg";
        BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);

        await Console.Out.WriteLineAsync($"Blob Url:\t{blobClient.Uri}");
    }

    /// create a new private static method named EnumerateContainersAsync that's asynchronous and has a single parameter of type BlobServiceClient
    /// EnumerateContainersAsync method, create an asynchronous foreach loop that iterates over the results of an invocation of the 
    // GetBlobContainersAsync method of the BlobServiceClient class and prints out the name of each container
    private static async Task EnumerateContainersAsync(BlobServiceClient client)
    {
        await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
        {
            await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
        }
    }

    /// In the Program class, create a new private static method named EnumerateBlobsAsync that's asynchronous and has two types of parameters, BlobServiceClient and string
    
    private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
    {   
        // In the EnumerateBlobsAsync method, get a new instance of the BlobContainerClient class by using the GetBlobContainerClient method of the BlobServiceClient class, passing in the containerName parameter   
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        
        // In the EnumerateBlobsAsync method, render the name of the container that will be enumerated
        await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");
        
        // In the EnumerateBlobsAsync method, create an asynchronous foreach loop that iterates over the results of an 
        // invocation of the GetBlobsAsync method of the BlobContainerClient class and prints out the name of each blob
        await foreach (BlobItem blob in container.GetBlobsAsync())
        {        
             await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
        }
    }

    // In the Program class, create a new private static method named GetContainerAsync that's asynchronous and has two parameter types, BlobServiceClient and string
    private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
    {      
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        
        await container.CreateIfNotExistsAsync();
        
        await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");
        
        return container;
    }

    private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
    {      
        BlobClient blob = client.GetBlobClient(blobName);
        await Console.Out.WriteLineAsync($"Blob Found:\t{blob.Name}");
        return blob;
    }
}