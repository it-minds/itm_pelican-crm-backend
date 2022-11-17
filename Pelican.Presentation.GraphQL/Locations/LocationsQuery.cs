using MediatR;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Application.Locations.Queries.GetLocations;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Presentation.GraphQL.Locations;
[ExtendObjectType("Query")]

public class LocationsQuery
{
	//This Query reguests all Locations from the database.
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<Location>> GetLocations([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetLocationsQuery(), cancellationToken);
	}
	//This Query reguests a specific Location from the database.
	public async Task<Location> GetLocationAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetLocationByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}
