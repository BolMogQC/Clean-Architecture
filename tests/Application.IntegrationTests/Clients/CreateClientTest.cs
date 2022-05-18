using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common.DataBaseFakers;
using CleanArchitecture.Application.Clients.Queries;
using CleanArchitecture.Application.IntegrationTests;
using NUnit.Framework;

namespace Application.IntegrationTests.Clients;

using static Testing;

public class CreateClientTest : TestBase
{
    [Test]
    public async Task Run()
    {
        var fakeClients = FakerManager.ClientFaker.Generate(10);
        await FakerManager.ClientFaker.SaveChangesAsync(fakeClients);
        
        var clients = await SendAsync(new GetClientsQuery());
        Console.WriteLine("Test");
    }
}