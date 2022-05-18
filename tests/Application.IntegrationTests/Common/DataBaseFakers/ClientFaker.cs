using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;

namespace Application.IntegrationTests.Common.DataBaseFakers;

public class ClientFaker : FakerBase<Client>
{
    public override IEnumerable<Client> Generate(int amount = 1)
    {
        _faker
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Phone, "8192669971");
        
        var values = _faker.Generate(amount);
        values.ForEach(x =>x.Address = FakerManager.AddressFaker.Generate().First());

        return values;
    }
}