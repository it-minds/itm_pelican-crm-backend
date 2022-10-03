using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL.DataLoader;

namespace Pelican.Presentation.GraphQL.Contacts;

[ExtendObjectType("Query")]
public class ContactsQuery
{
	[UseDbContext(typeof(PelicanContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Contact> GetContacts([ScopedService] PelicanContext context) => context.Contacts.AsNoTracking();

	[Authorize]
	public Task<Contact> GetClientAsync(Guid id, ContactByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
