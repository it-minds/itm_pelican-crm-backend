using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class AccountManagerDealsRepository : GenericRepository<AccountManagerDeal>
{
	public AccountManagerDealsRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<AccountManagerDeal> FindAllWithIncludes()
	{
		return PelicanContext.Set<AccountManagerDeal>().AsNoTracking()
			.Include(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.Deal)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.Include(x => x.Deal)
			.ThenInclude(x => x.Client!)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts);
	}
}
