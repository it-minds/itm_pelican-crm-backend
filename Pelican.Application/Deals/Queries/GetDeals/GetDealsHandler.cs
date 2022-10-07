using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Deals.Queries.GetDeals;
public class GetLocationsHandler : IRequestHandler<GetDealsQuery, IQueryable<Deal>>
{
	private readonly IDealRepository _repository;
	public GetLocationsHandler(IDealRepository dealRepository)
	{
		_repository = dealRepository;
	}
	public async Task<IQueryable<Deal>> Handle(GetDealsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
