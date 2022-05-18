using FluentValidation;

namespace CleanArchitecture.Application.Clients.Commands.CreateClientCommand;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        // Required Fields
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        
        // Maximum Length
        RuleFor(x => x.Name).MaximumLength(50);
        RuleFor(x => x.Email).MaximumLength(50);
        RuleFor(x => x.Phone).MaximumLength(10);
        RuleFor(x => x.Street).MaximumLength(100);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.Country).MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.ZipCode).MaximumLength(50);
    }
}