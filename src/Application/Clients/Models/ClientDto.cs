using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.Clients.Models;

public class ClientDto : IMapFrom<Client>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}