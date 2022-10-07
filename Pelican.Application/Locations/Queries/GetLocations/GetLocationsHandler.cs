using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Locations.Queries.GetLocations;
public class GetLocationsHandler : IRequestHandler<GetLocationsQuery, IQueryable<Location>>
{
	private readonly ILocationRepository _repository;
	public GetLocationsHandler(ILocationRepository dealRepository)
	{
		_repository = dealRepository;
	}
	public async Task<IQueryable<Location>> Handle(GetLocationsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
