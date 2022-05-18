using AutoMapper;
using CleanArchitecture.Application.Clients.Models;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Clients.Queries;

public class GetClientsQuery : IRequest<List<ClientDto>>
{
}

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Clients.AsNoTracking().ProjectToListAsync<ClientDto>(_mapper.ConfigurationProvider);
    }
}