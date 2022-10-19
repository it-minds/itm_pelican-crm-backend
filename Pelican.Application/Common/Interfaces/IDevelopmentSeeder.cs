using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Common.Interfaces;
public interface IDevelopmentSeeder
{
	public void SeedEntireDb(int count);
	public IQueryable<Location> SeedLocations(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count);
	public IQueryable<Supplier> SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> locations, int count);
	public IQueryable<AccountManager> SeedAccountManagers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count);
	public IQueryable<Client> SeedClients(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> location, int count);
	public IQueryable<Deal> SeedDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> client, int count);
	public IQueryable<Contact> SeedContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count);
	public IQueryable<AccountManagerDeal> SeedAccountManagerDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals);
	public IQueryable<DealContact> SeedDealContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Deal> deals, IQueryable<Contact> contacts);
	public IQueryable<ClientContact> SeedClientContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> clients, IQueryable<Contact> contacts);
}
