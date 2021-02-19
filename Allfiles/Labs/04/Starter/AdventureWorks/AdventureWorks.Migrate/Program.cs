// directives for libraries that will be referenced by the application
using AdventureWorks.Context;
using AdventureWorks.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// new Program class with two constant string properties and an asynchronous Main entry point method
public class Program
{
    private const string sqlDBConnectionString = "Server=tcp:polysqlsrvks.database.windows.net,1433;Initial Catalog=AdventureWorks;Persist Security Info=False;User ID=testuser;Password=TestPa55w.rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    private const string cosmosDBConnectionString = "AccountEndpoint=https://polycosmosks.documents.azure.com:443/;AccountKey=ee4bg3v4GSOv35IsGPtXWPt6dFVNDqzL2B0daOvKO0unJmSJFwo28LSx2p0Jkyhrl0vxqNZ4CjRju2f7ExlC4Q==;";
    
    public static async Task Main(string[] args)
    {
        // Within the Main method, add the following blocks of code to export all model and product records from SQL Database to local memory
        await Console.Out.WriteLineAsync("Start Migration");

        using AdventureWorksSqlContext context = new AdventureWorksSqlContext(sqlDBConnectionString);

        List<Model> items = await context.Models
            .Include(m => m.Products)
            .ToListAsync<Model>();

        await Console.Out.WriteLineAsync($"Total Azure SQL DB Records: {items.Count}");

        // Still within the Main method, add the following blocks of code to import the in-memory model and product data as documents to Azure Cosmos DB
        using CosmosClient client = new CosmosClient(cosmosDBConnectionString);

        Database database = await client.CreateDatabaseIfNotExistsAsync("Retail");

        Container container = await database.CreateContainerIfNotExistsAsync("Online",
            partitionKeyPath: $"/{nameof(Model.Category)}",
            throughput: 1000
        );

        int count = 0;
        foreach (var item in items)
        {
            ItemResponse<Model> document = await container.UpsertItemAsync<Model>(item);
            await Console.Out.WriteLineAsync($"Upserted document #{++count:000} [Activity Id: {document.ActivityId}]");
        }

        await Console.Out.WriteLineAsync($"Total Azure Cosmos DB Documents: {count}");
    }
}