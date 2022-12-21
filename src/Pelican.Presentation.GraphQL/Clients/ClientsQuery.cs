using MediatR;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Presentation.GraphQL.Clients;

[Authorized(Role = RoleEnum.Standard)]
[Authorized(Role = RoleEnum.Admin)]
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

