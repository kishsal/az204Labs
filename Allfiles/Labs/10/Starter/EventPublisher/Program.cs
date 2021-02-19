// Using directives
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//new Program class with two constant string properties named topicEndpoint
//and topicKey, and then create an asynchronous Main entry point method
public class Program
{
    private const string topicEndpoint = "https://hrtopicks.eastus-1.eventgrid.azure.net/api/events";
    private const string topicKey = "G2LzYqnaj/IBTmrz4Ad57iXuHkgQ+FI30fzoESTxdH8=";
    
    public static async Task Main(string[] args)
    {
        // following block of code to connect to the Event Grid using the credentials you specified earlier in the lab
        TopicCredentials credentials = new TopicCredentials(topicKey);
        EventGridClient client = new EventGridClient(credentials);

        // new variable named events, of type Lis
        List<EventGridEvent> events = new List<EventGridEvent>();

        //Add the following block of code to: create two new variables named firstPerson of an anonymous type, and firstEvent 
        //of type EventGridEvent; populate the EventGridEvent variable with sample data; and add the firstEvent instance to your events list
        var firstPerson = new
        {
            FullName = "Alba Sutton",
            Address = "4567 Pine Avenue, Edison, WA 97202"
        };    
            
        EventGridEvent firstEvent = new EventGridEvent
        {
            Id = Guid.NewGuid().ToString(),
            EventType = "Employees.Registration.New",
            EventTime = DateTime.Now,
            Subject = $"New Employee: {firstPerson.FullName}",
            Data = firstPerson.ToString(),
            DataVersion = "1.0.0"
        };
        events.Add(firstEvent);

        // Add the following block of code to: create two new variables named secondPerson of an anonymous type, and secondEvent of type 
        //EventGridEvent; populate the EventGridEvent variable with sample data; and add the secondEvent instance to your events list
        var secondPerson = new
        {
            FullName = "Alexandre Doyon",
            Address = "456 College Street, Bow, WA 98107"
        };

        EventGridEvent secondEvent = new EventGridEvent
        {
            Id = Guid.NewGuid().ToString(),
            EventType = "Employees.Registration.New",
            EventTime = DateTime.Now,
            Subject = $"New Employee: {secondPerson.FullName}",
            Data = secondPerson.ToString(),
            DataVersion = "1.0.0"
        };
        events.Add(secondEvent);

        var thirdPerson = new
        {
            FullName = "Jessica Alba",
            Address = "4437 Pine Ave, Edison, WA 97202"
        };    
            
        EventGridEvent thirdEvent = new EventGridEvent
        {
            Id = Guid.NewGuid().ToString(),
            EventType = "Employees.Registration.New",
            EventTime = DateTime.Now,
            Subject = $"New Employee: {thirdPerson.FullName}",
            Data = thirdPerson.ToString(),
            DataVersion = "1.0.0"
        };
        events.Add(thirdEvent);
        
        // Add the following block of code to obtain the Hostname from the topicEndpoint variable, 
        //and then use that hostname as a parameter to the EventGridClient.PublishEventsAsync method invocation
        string topicHostname = new Uri(topicEndpoint).Host;
        await client.PublishEventsAsync(topicHostname, events);

        // Render the Events published message to the console
        Console.WriteLine("Events published");
    }
}