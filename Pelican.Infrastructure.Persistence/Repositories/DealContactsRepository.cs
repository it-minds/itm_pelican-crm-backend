using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class DealContactsRepository : GenericRepository<DealContact>
{
	public DealContactsRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<DealContact> FindAllWithIncludes()
	{
		return PelicanContext.Set<DealContact>().AsNoTracking()
			.Include(x => x.Deal)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.Deal)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.Include(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.Deals)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations);
	}
}
