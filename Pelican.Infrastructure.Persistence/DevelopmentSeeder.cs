using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence;
public class DevelopmentSeeder
{
	//This method is only partially created it should call specific methods to seed each table in the database.
	private IUnitOfWork _unitOfWork;
	private IPelicanBogusFaker _faker;
	public DevelopmentSeeder(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker)
	{
		_unitOfWork = unitOfWork;
		_faker = pelicanFaker;
	}
	public async void SeedEntireDb(int count)
	{
		var locations = SeedLocations(_unitOfWork, _faker, count);
		var suppliers = SeedSuppliers(_unitOfWork, _faker, locations, count);
		var accountManagers = SeedAccountManagers(_unitOfWork, _faker, suppliers, count);
		var clients = SeedClients(_unitOfWork, _faker, locations, count);
		var deals = SeedDeals(_unitOfWork, _faker, clients, count);
		var contact = SeedContacts(_unitOfWork, _faker, count);
		await _unitOfWork.SaveAsync();
	}
	public IQueryable<Location> SeedLocations(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		var variable = unitOfWork.LocationRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.LocationFaker(count);
		unitOfWork.LocationRepository
			.CreateRange(result);
		return result.AsQueryable();
	}
	//This Method calls on the PelicanBogusFaker to give it a list of 5 Supplier entities and saves these to the database.
	public IQueryable<Supplier> SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> locations, int count)
	{
		var variable = unitOfWork.SupplierRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.SupplierFaker(count, locations);
		unitOfWork.SupplierRepository
			.CreateRange(result);
		return result.AsQueryable();
	}
	public IQueryable<AccountManager> SeedAccountManagers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count)
	{
		var variable = unitOfWork.AccountManagerRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.AccountManagerFaker(count, supplier);
		unitOfWork.AccountManagerRepository
			.CreateRange(result);
		return result.AsQueryable();
	}
	public IQueryable<Client> SeedClients(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> location, int count)
	{
		var variable = unitOfWork.ClientRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.ClientFaker(count, location);
		unitOfWork.ClientRepository
			.CreateRange(result);
		return result.AsQueryable();
	}
	public IQueryable<Deal> SeedDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> client, int count)
	{
		var variable = unitOfWork.DealRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.DealFaker(count, client);
		unitOfWork.DealRepository
			.CreateRange(result);
		return result.AsQueryable();


	}
	public IQueryable<Contact> SeedContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		var variable = unitOfWork.ContactRepository.FindAll();
		if (variable.Any())
			return variable;
		var result = pelicanFaker.ContactFaker(count);
		unitOfWork
			.ContactRepository
			.CreateRange(result);
		return result.AsQueryable();
	}
}
