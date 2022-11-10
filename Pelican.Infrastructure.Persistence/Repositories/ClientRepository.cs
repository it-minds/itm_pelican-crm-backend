using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ClientRepository : GenericRepository<Client>
{
	public ClientRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

	public override IQueryable<Client> FindAllWithIncludes()
	{
		Console.WriteLine("FindAllWithIncludesForClient");
		var client = PelicanContext.Set<Client>().AsNoTracking()
			.Include(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations)
			.Include(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Deal)
			.Include(x => x.Deals)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.AccountManager)
			.ThenInclude(x => x.Supplier)
			.ThenInclude(x => x.OfficeLocations);
		Console.WriteLine(client.FirstOrDefault().Name);
		return client;
	}
}
