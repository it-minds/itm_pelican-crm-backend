using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class AccountManagerDealRepository : RepositoryBase<AccountManagerDeal>, IAccountManagerDealRepository
{
	public AccountManagerDealRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{

	}
}

