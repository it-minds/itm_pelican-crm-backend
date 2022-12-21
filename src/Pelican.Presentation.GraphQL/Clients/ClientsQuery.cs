using HotChocolate.AspNetCore.Authorization;
using MediatR;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Clients;

[Authorize(Roles = new[] { Roles.Admin, Roles.Standard })]
[ExtendObjectType("Query")]
public class ClientsQuery
{
	//This Query reguests all Clients from the database.
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<Client>> GetClients([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetClientsQuery(), cancellationToken);
	}
	//This Query reguests a specific Client from the database.
	public async Task<Client> GetClientAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetClientByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}

