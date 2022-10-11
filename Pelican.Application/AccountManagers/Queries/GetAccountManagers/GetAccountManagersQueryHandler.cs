using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public class GetAccountManagersQueryHandler : IRequestHandler<GetAccountManagersQuery, IQueryable<AccountManager>>
{
	private readonly IRepositoryBase<AccountManager> _repository;
	public GetAccountManagersQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.AccountManagerRepository;
	}
	//Uses the repository for AccountManager to find all AccountManagers in the database
	public async Task<IQueryable<AccountManager>> Handle(GetAccountManagersQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
