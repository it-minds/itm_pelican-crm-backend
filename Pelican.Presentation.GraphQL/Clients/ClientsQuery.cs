using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL.DataLoader;

namespace Pelican.Presentation.GraphQL.Clients;
[ExtendObjectType("Query")]
public class ClientsQuery
{
	[UseDbContext(typeof(PelicanContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Client> GetClients([ScopedService] PelicanContext context) => context.Clients.AsNoTracking();


	[Authorize]
	public Task<Client> GetClientAsync(Guid id, ClientByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
