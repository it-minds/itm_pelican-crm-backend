using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.GraphQL.Deals;
[ExtendObjectType("Query")]
public class DealsQuery
{
	[UseDbContext(typeof(PelicanContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Deal> GetDeals([ScopedService] PelicanContext context) => context.Deals.AsNoTracking();

	[Authorize]
	public Task<Deal> GetDealAsync(Guid id, DealByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
