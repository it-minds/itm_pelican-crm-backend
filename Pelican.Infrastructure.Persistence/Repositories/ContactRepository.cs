using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
internal class ContactRepository : GenericRepository<Contact>
{
	public ContactRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<Contact> FindAllWithIncludes()
	{
		return PelicanContext.Set<Contact>().AsNoTracking()
			.Include(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.ClientContacts)
			.Include(x => x.ClientContacts)
			.ThenInclude(x => x.Client)
			.ThenInclude(x => x.Deals)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
;
	}
}
