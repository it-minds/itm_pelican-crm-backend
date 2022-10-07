using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Locations.Queries.GetLocations;
public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IQueryable<Location>>
{
	private readonly ILocationRepository _repository;
	public GetLocationsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.LocationRepository;
	}
	public async Task<IQueryable<Location>> Handle(GetLocationsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
