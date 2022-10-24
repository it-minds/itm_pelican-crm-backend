using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ClientContactsRepository : GenericRepository<ClientContact>
{
	public ClientContactsRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<ClientContact> FindAllWithIncludes()
	{
		return PelicanContext.Set<ClientContact>().AsNoTracking()
			.Include(x => x.Contact)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.Contact)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client)
			.Include(x => x.Client)
			.ThenInclude(x => x.Deals)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations);
	}
}
