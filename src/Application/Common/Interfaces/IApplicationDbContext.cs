using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Client> Clients { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
