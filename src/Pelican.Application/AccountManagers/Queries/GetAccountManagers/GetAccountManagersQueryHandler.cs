using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public class GetAccountManagersQueryHandler : IQueryHandler<GetAccountManagersQuery, IQueryable<AccountManager>>
{
	private readonly IGenericRepository<AccountManager> _repository;
	public GetAccountManagersQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.AccountManagerRepository;
	}
	//Uses the repository for AccountManager to find all AccountManagers in the database
	public async Task<IQueryable<AccountManager>> Handle(GetAccountManagersQuery request, CancellationToken cancellation)
	{
		return await Task.Run(() => _repository.FindAll());
	}
}
