using MediatR;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Application.Locations.Queries.GetLocations;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Presentation.GraphQL.Locations;
[ExtendObjectType("Query")]

public class LocationsQuery
{
	public async Task<IQueryable<Location>> GetLocatíons([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetLocationsQuery(), cancellationToken);
	}

	public async Task<Location> GetLocationAsync(GetLocationByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
