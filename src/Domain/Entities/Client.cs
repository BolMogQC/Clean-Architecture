namespace CleanArchitecture.Domain.Entities;

public class Client : AuditableEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}