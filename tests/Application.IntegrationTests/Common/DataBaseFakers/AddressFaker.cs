using System.Collections.Generic;
using CleanArchitecture.Domain.ValueObjects;

namespace Application.IntegrationTests.Common.DataBaseFakers;

public class AddressFaker : FakerBase<Address>
{
    public override IEnumerable<Address> Generate(int amount = 1)
    {
        _faker
            .RuleFor(x => x.Street, f => f.Address.StreetAddress())
            .RuleFor(x => x.City, f => f.Address.City())
            .RuleFor(x => x.State, f => f.Address.State())
            .RuleFor(x => x.ZipCode, f => f.Address.ZipCode())
            .RuleFor(x => x.Country, f => f.Address.Country());
        
        return _faker.Generate(amount);
    }
}