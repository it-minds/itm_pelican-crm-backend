using MediatR;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Clients;
[ExtendObjectType("Query")]
public class ClientsQuery
{
	//This Query reguests all Clients from the database.
	[UsePaging]
	public async Task<IQueryable<Client>> GetClients([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		Console.WriteLine("GetClients");
		try
		{
			await mediator.Send(new GetClientsQuery(), cancellationToken);
			Console.WriteLine("Got a client");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Console.WriteLine(ex.StackTrace);
		}
		var client = await mediator.Send(new GetClientsQuery(), cancellationToken);
		return client;
	}
	//This Query reguests a specific Client from the database.
	public async Task<Client> GetClientAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetClientByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}

