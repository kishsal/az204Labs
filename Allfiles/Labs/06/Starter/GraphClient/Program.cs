//  using directives for libraries that will be referenced by the application
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;    
using Microsoft.Graph.Auth;

// new Program class with two constant string properties named _clientId and _tenantId, and then create an asynchronous Main entry point method
public class Program
{
    private const string _clientId = "423e676b-a6bb-4a9b-85b5-2900b5c7682f";
    private const string _tenantId = "eca63cb6-2016-49c5-936f-de4b04a1d4e5";
    
    public static async Task Main(string[] args)
    {
        // Create a new variable named app of type IPublicClientApplication
        IPublicClientApplication app;

        // Add the following block of code to build a public client application instance by using the static PublicClientApplicationBuilder class, and then store it in the app variable
        app = PublicClientApplicationBuilder
        .Create(_clientId)
        .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
        .WithRedirectUri("http://localhost")
        .Build();

        // Add the following block of code to create a new generic string List<> with a single value of user.read
        List<string> scopes = new List<string> 
        { 
            "user.read" 
        };

        // // Create a new variable named result of type AuthenticationResult
        // AuthenticationResult result;

        // //Add the following block of code to acquire a token interactively and store the output in the result variable
        // result = await app
        // .AcquireTokenInteractive(scopes)
        // .ExecuteAsync();

        // // Render the value of the AuthenticationResult.AccessToken member to the console
        // Console.WriteLine($"Token:\t{result.AccessToken}");

        // Create a new variable named provider of type DeviceCodeProvider that passes in the variables app, and scopes as constructor parameters
        DeviceCodeProvider provider = new DeviceCodeProvider(app, scopes);

        // Create a new variable named client of type GraphServiceClient that passes in the variable provider as a constructor parameter
        GraphServiceClient client = new GraphServiceClient(provider);

        // Add the following block of code to use the GraphServiceClient instance to asynchronously get the response of issuing an HTTP 
        //request to the relative /Me directory of the REST API, and then store the result in a new variable named myProfile of type User
        User myProfile = await client.Me
        .Request()
        .GetAsync();

        // Render the value of the User.DisplayName and User.Id members to the console
        Console.WriteLine($"Name:\t{myProfile.DisplayName}");
        Console.WriteLine($"AAD Id:\t{myProfile.Id}");
    }
}