using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class DealRepository : GenericRepository<Deal>
{
	public DealRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<Deal> FindAllWithIncludes()
	{
		return PelicanContext.Set<Deal>().AsNoTracking()
			.Include(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.Include(x => x.Client)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts);
	}
}
