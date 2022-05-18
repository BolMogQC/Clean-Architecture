using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(10);
        
        builder.OwnsOne(o => o.Address);
        builder.OwnsOne(o => o.Address).Property(p => p.Street).HasMaxLength(200).IsRequired();
        builder.OwnsOne(o => o.Address).Property(p => p.Street).HasColumnName("Street");
        builder.OwnsOne(o => o.Address).Property(p => p.City).HasMaxLength(100).IsRequired();
        builder.OwnsOne(o => o.Address).Property(p => p.City).HasColumnName("City");
        builder.OwnsOne(o => o.Address).Property(p => p.State).HasMaxLength(100).IsRequired();
        builder.OwnsOne(o => o.Address).Property(p => p.State).HasColumnName("State");
        builder.OwnsOne(o => o.Address).Property(p => p.Country).HasMaxLength(100).IsRequired();
        builder.OwnsOne(o => o.Address).Property(p => p.Country).HasColumnName("Country");
        builder.OwnsOne(o => o.Address).Property(p => p.ZipCode).HasMaxLength(50).IsRequired();
        builder.OwnsOne(o => o.Address).Property(p => p.ZipCode).HasColumnName("ZipCode");
    }
}