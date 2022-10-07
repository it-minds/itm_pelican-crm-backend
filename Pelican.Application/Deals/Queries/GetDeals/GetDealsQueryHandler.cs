using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Deals.Queries.GetDeals;
public class GetDealsQueryHandler : IRequestHandler<GetDealsQuery, IQueryable<Deal>>
{
	private readonly IDealRepository _repository;
	public GetDealsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.DealRepository;
	}
	public async Task<IQueryable<Deal>> Handle(GetDealsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
