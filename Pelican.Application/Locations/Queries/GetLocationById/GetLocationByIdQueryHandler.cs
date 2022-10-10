using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocationById;
public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Location>
{
	private readonly ILocationByIdDataLoader _dataLoader;
	public GetLocationByIdQueryHandler(ILocationByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Location in the database using their Id
	public async Task<Location> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
