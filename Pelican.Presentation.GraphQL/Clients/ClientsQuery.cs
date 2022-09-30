using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL.DataLoader;
using Pelican.Presentation.GraphQL.Extensions;

namespace Pelican.Presentation.GraphQL.Clients;
[ExtendObjectType("Query")]
public class ClientsQuery
{
	[UseApplicationDbContext]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Client> GetClients([ScopedService] PelicanContext context) => context.Clients.AsNoTracking();

	[UseApplicationDbContext]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public Task<Client> GetClientAsync(Guid id, ClientByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
