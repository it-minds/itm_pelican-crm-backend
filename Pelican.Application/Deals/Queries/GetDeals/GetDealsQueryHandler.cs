using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDeals;
public class GetDealsQueryHandler : IQueryHandler<GetDealsQuery, IQueryable<Deal>>
{
	private readonly IGenericRepository<Deal> _repository;
	public GetDealsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.DealRepository;
	}
	//Uses the repository for Deal to find all Deals in the database
	public async Task<IQueryable<Deal>> Handle(GetDealsQuery request, CancellationToken cancellation)
	{
		return await Task.Run(() => _repository.FindAll());
	}
}
