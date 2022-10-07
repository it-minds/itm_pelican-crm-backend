using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public class GetAccountManagersQueryHandler : IRequestHandler<GetAccountManagersQuery, IQueryable<AccountManager>>
{
	private readonly IAccountManagerRepository _repository;
	public GetAccountManagersQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.AccountManagerRepository;
	}
	public async Task<IQueryable<AccountManager>> Handle(GetAccountManagersQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
