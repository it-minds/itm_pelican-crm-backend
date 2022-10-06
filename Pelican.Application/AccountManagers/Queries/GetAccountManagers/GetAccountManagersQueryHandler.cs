using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public class GetClientsQueryHandler : IRequestHandler<GetAccountManagersQuery, IQueryable<AccountManager>>
{
	private readonly IAccountManagerRepository _repository;
	public GetClientsQueryHandler(IAccountManagerRepository accountManagerRepository)
	{
		_repository = accountManagerRepository;
	}
	public async Task<IQueryable<AccountManager>> Handle(GetAccountManagersQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
