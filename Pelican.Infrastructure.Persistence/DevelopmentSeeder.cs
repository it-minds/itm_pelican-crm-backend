using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence;
public class DevelopmentSeeder : IDevelopmentSeeder
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPelicanBogusFaker _faker;
	private readonly CancellationToken cancellationToken;
	public DevelopmentSeeder(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker)
	{
		_unitOfWork = unitOfWork;
		_faker = pelicanFaker;
		cancellationToken = new CancellationToken();
	}

	public async void SeedEntireDb(int count)
	{
		var suppliers = SeedSuppliers(_unitOfWork, _faker, count);
		var locations = SeedLocations(_unitOfWork, _faker, suppliers, count);
		var accountManagers = SeedAccountManagers(_unitOfWork, _faker, suppliers, count);
		var clients = SeedClients(_unitOfWork, _faker, locations, count);
		var deals = SeedDeals(_unitOfWork, _faker, clients, accountManagers, count);
		var contacts = SeedContacts(_unitOfWork, _faker, accountManagers, count);
		var accountManagerDeals = SeedAccountManagerDeals(_unitOfWork, _faker, accountManagers, deals);
		var clientContacts = SeedClientContacts(_unitOfWork, _faker, clients, contacts);
		var dealContacts = SeedDealContacts(_unitOfWork, _faker, deals, contacts);

		await _unitOfWork.SaveAsync(cancellationToken);
	}

	public IQueryable<Location> SeedLocations(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> suppliers, int count)
	{
		var repositoryContent = unitOfWork.LocationRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.LocationFaker(count, suppliers);
		unitOfWork.LocationRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}

	public IQueryable<Supplier> SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, int count)
	{
		var repositoryContent = unitOfWork.SupplierRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.SupplierFaker(count);
		unitOfWork.SupplierRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}

	public IQueryable<AccountManager> SeedAccountManagers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Supplier> supplier, int count)
	{
		var repositoryContent = unitOfWork.AccountManagerRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.AccountManagerFaker(count, supplier);
		unitOfWork.AccountManagerRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}

	public IQueryable<Client> SeedClients(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Location> location, int count)
	{
		var repositoryContent = unitOfWork.ClientRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.ClientFaker(count, location);
		unitOfWork.ClientRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}
	public IQueryable<Deal> SeedDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> client, IQueryable<AccountManager> accountManagers, int count)
	{
		var repositoryContent = unitOfWork.DealRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.DealFaker(count, client, accountManagers);
		unitOfWork.DealRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();


	}

	public IQueryable<Contact> SeedContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<AccountManager> accountManagers, int count)
	{
		var repositoryContent = unitOfWork.ContactRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.ContactFaker(count, accountManagers);
		unitOfWork
			.ContactRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}

	public IQueryable<AccountManagerDeal> SeedAccountManagerDeals(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals)
	{
		var repositoryContent = unitOfWork.AccountManagerDealRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.AccountManagerDealFaker(accountManagers, deals);
		unitOfWork.AccountManagerDealRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}
	public IQueryable<DealContact> SeedDealContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Deal> deals, IQueryable<Contact> contacts)
	{
		var repositoryContent = unitOfWork.DealContactRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.DealContactFaker(deals, contacts);
		unitOfWork.DealContactRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}

	public IQueryable<ClientContact> SeedClientContacts(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker, IQueryable<Client> clients, IQueryable<Contact> contacts)
	{
		var repositoryContent = unitOfWork.ClientContactRepository.FindAll();
		if (repositoryContent.Any())
			return repositoryContent;
		var result = pelicanFaker.ClientContactFaker(clients, contacts);
		unitOfWork.ClientContactRepository
			.CreateRangeAsync(result, cancellationToken);
		return result.AsQueryable();
	}
}
