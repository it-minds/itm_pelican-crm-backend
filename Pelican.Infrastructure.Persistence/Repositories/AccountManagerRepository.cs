using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class AccountManagerRepository : RepositoryBase<AccountManager>, IAccountManagerRepository
{
	public AccountManagerRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
