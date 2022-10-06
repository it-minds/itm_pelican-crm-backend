using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Infrastructure.Persistence;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Presentation.GraphQL.Locations;
[ExtendObjectType("Query")]

public class LocationsQuery
{
	[UseDbContext(typeof(IDbContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Location> GetLocations([ScopedService] IDbContext context) => context.Locations.AsNoTracking();

	[Authorize]
	public Task<Location> GetLocationAsync(Guid id, LocationByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
