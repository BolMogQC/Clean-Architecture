using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Common.DataBaseFakers;

public abstract class FakerBase<T> where T : class
{
    protected readonly Faker<T> _faker = new();

    public abstract IEnumerable<T> Generate(int amount = 1);

    public async Task SaveChangesAsync(IEnumerable<T> entities)
    {
        using var scope = Testing.ScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Set<T>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}