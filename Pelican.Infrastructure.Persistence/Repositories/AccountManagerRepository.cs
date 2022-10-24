using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class AccountManagerRepository : GenericRepository<AccountManager>
{
	public AccountManagerRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<AccountManager> FindAllWithIncludes()
	{
		return PelicanContext.Set<AccountManager>().AsNoTracking()
			.Include(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.Include(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts);
	}
}
