using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class SupplierRepository : GenericRepository<Supplier>
{
	public SupplierRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<Supplier> FindAllWithIncludes()
	{
		return PelicanContext.Set<Supplier>().AsNoTracking()
			.Include(x => x.OfficeLocations)
			.Include(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.Include(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client!)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts);
	}
}
