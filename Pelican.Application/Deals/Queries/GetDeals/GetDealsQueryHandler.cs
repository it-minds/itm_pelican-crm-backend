using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Deals.Queries.GetDeals;
public class GetDealsQueryHandler : IRequestHandler<GetDealsQuery, IQueryable<Deal>>
{
	private readonly IRepositoryBase<Deal> _repository;
	public GetDealsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.DealRepository;
	}
	//Uses the repository for Deal to find all Deals in the database
	public async Task<IQueryable<Deal>> Handle(GetDealsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
