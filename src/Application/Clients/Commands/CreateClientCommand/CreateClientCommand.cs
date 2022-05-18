using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using MediatR;

namespace CleanArchitecture.Application.Clients.Commands.CreateClientCommand;

public class CreateClientCommand : IRequest<string>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
}

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, string>
{
    private readonly IApplicationDbContext context;

    public CreateClientCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }
    
    public async Task<string> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var entity = new Client
        {
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            Address = new Address(command.Street, command.City, command.State, command.Country, command.ZipCode)
        };
        
        await context.Clients.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return entity.Id;
    }
}