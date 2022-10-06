using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Deals;
[ExtendObjectType("Query")]
public class DealsQuery
{
	[UseDbContext(typeof(IDbContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Deal> GetDeals([ScopedService] IDbContext context) => context.Deals.AsNoTracking();

	[Authorize]
	public Task<Deal> GetDealAsync(Guid id, DealByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
