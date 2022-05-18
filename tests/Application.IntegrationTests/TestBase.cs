using System.Threading.Tasks;
using Application.IntegrationTests;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests;

using static Testing;

public class TestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}