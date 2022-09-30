using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL.DataLoader;
using Pelican.Presentation.GraphQL.Extensions;

namespace Pelican.Presentation.GraphQL.ClientContacts;
public class ClientContactsQuery
{
	[UseApplicationDbContext]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<ClientContact> GetClientContacts([ScopedService] PelicanContext context) => context.ClientContacts.AsNoTracking();

	[UseApplicationDbContext]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public Task<ClientContact> GetClientContactsAsync(Guid id, ClientContactsByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
