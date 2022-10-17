using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence;
public static class DevelopmentSeeder
{
	//This method is only partially created it should call specific methods to seed each table in the database.
	public static void SeedEntireDb(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		var locations = SeedLocations(unitOfWork, pelicanFaker, count);
		var suppliers = SeedSuppliers(unitOfWork, pelicanFaker, locations, count);
		var accountManagers = SeedAccountManagers(unitOfWork, pelicanFaker, suppliers, count);
		var clients = SeedClients(unitOfWork, pelicanFaker, locations, count);
		var deals = SeedDeals(unitOfWork, pelicanFaker, clients, count);
		var contact = SeedContacts(unitOfWork, pelicanFaker, count);
	}
	public static IQueryable<Location> SeedLocations(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		unitOfWork
			.LocationRepository
			.CreateRange(pelicanFaker.LocationFaker(count));
		unitOfWork.SaveAsync();
		var locationList = unitOfWork.LocationRepository.FindAll();
		return locationList;
	}
	//This Method calls on the PelicanBogusFaker to give it a list of 5 Supplier entities and saves these to the database.
	public static IQueryable<Supplier> SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> locations, int count)
	{
		unitOfWork
			.SupplierRepository
			.CreateRange(pelicanFaker.SupplierFaker(count, locations));
		unitOfWork.SaveAsync();
		var supplierList = unitOfWork.SupplierRepository.FindAll();
		return supplierList;
	}
	public static IQueryable<AccountManager> SeedAccountManagers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count)
	{
		unitOfWork
			.AccountManagerRepository
			.CreateRange(pelicanFaker.AccountManagerFaker(count, supplier));
		unitOfWork.SaveAsync();
		var accountManagerList = unitOfWork.AccountManagerRepository.FindAll();
		return accountManagerList;
	}
	public static IQueryable<Client> SeedClients(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> location, int count)
	{
		unitOfWork.ClientRepository
			.CreateRange(pelicanFaker
			.ClientFaker(count, location));
		unitOfWork.SaveAsync();
		var clientList = unitOfWork.ClientRepository.FindAll();
		return clientList;
	}
	public static IQueryable<Deal> SeedDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> client, int count)
	{
		unitOfWork.DealRepository
			.CreateRange(pelicanFaker
			.DealFaker(count, client));
		unitOfWork.SaveAsync();
		var dealList = unitOfWork.DealRepository.FindAll();
		return dealList;
	}
	public static IQueryable<Contact> SeedContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		unitOfWork
			.ContactRepository
			.CreateRange(pelicanFaker.ContactFaker(count));
		unitOfWork.SaveAsync();
		var contactList = unitOfWork.ContactRepository.FindAll();
		return contactList;
	}
}
