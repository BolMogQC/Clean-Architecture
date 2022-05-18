using CleanArchitecture.Application.Clients.Commands.CreateClientCommand;
using CleanArchitecture.Application.Clients.Models;
using CleanArchitecture.Application.Clients.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers.Api;

public class ClientsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> Get()
    {
        return await Mediator.Send(new GetClientsQuery());
    }

    [HttpPost]
    public async Task<string> CreateClient([FromBody] CreateClientCommand command)
    {
        return await Mediator.Send(command);
    }
}