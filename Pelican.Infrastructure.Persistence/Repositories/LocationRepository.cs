using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Repositories;
internal class LocationRepository : GenericRepository<Location>
{
	public LocationRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<Location> FindAllWithIncludes()
	{
		return PelicanContext.Set<Location>().AsNoTracking()
			.Include(x => x.Supplier)
			.ThenInclude(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.Include(x => x.Supplier)
			.ThenInclude(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts);
	}
}
