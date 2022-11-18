using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;

namespace Pelican.Application.Abstractions.Data;

public interface IDevelopmentSeeder
{
	public void SeedEntireDb(int count);
	public IQueryable<Location> SeedLocations(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count);
	public IQueryable<Supplier> SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count);
	public IQueryable<AccountManager> SeedAccountManagers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count);
	public IQueryable<Client> SeedClients(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> location, int count);
	public IQueryable<Deal> SeedDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> client, IQueryable<AccountManager> accountManagers, int count);
	public IQueryable<Contact> SeedContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<AccountManager> accountManagers, int count);
	public IQueryable<AccountManagerDeal> SeedAccountManagerDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals);
	public IQueryable<DealContact> SeedDealContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Deal> deals, IQueryable<Contact> contacts);
	public IQueryable<ClientContact> SeedClientContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> clients, IQueryable<Contact> contacts);
}
