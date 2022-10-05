using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.GraphQL.AccountManagers;
[ExtendObjectType("Query")]
public class AccountManagersQuery
{
	[UseDbContext(typeof(PelicanContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<AccountManager> GetAccountManagers([ScopedService] PelicanContext context) => context.AccountManagers.AsNoTracking();


	[Authorize]
	public Task<AccountManager> GetAccountManagerAsync(Guid id, AccountManagerByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
