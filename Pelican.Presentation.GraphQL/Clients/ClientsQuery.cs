using MediatR;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Clients;
[ExtendObjectType("Query")]
public class ClientsQuery
{
	//This Query reguests all Clients from the database.
	public async Task<IQueryable<Client>> GetClients([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetClientsQuery(), cancellationToken);
	}
	//This Query reguests a specific Client from the database.
	public async Task<Client> GetClientAsync(GetClientByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}

