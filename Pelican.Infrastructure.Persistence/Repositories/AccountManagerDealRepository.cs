using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class AccountManagerDealRepository : RepositoryBase<AccountManagerDeal>, IAccountManagerDealRepository
{
	public AccountManagerDealRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{

	}
}

