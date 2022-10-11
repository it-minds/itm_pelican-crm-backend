using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
	private IPelicanContext _pelicanContext;
	private IGenericRepository<AccountManagerDeal> accountManagerDealRepository;
	private IGenericRepository<AccountManager> accountManagerRepository;
	private IGenericRepository<Client> clientRepository;
	private IGenericRepository<Contact> contactRepository;
	private IGenericRepository<Deal> dealRepository;
	private IGenericRepository<Location> locationRepository;
	private IGenericRepository<Supplier> supplierRepository;


	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository
	{
		get
		{
			if (accountManagerDealRepository == null)
			{
				accountManagerDealRepository = new GenericRepository<AccountManagerDeal>(_pelicanContext);
			}
			return accountManagerDealRepository;
		}
	}

	public IGenericRepository<AccountManager> AccountManagerRepository
	{
		get
		{
			if (accountManagerRepository == null)
			{
				accountManagerRepository = new GenericRepository<AccountManager>(_pelicanContext);
			}
			return accountManagerRepository;
		}
	}

	public IGenericRepository<Client> ClientRepository
	{
		get
		{
			if (clientRepository == null)
			{
				clientRepository = new GenericRepository<Client>(_pelicanContext);
			}
			return clientRepository;
		}
	}

	public IGenericRepository<Contact> ContactRepository
	{
		get
		{
			if (contactRepository == null)
			{
				contactRepository = new GenericRepository<Contact>(_pelicanContext);
			}
			return contactRepository;
		}
	}

	public IGenericRepository<Deal> DealRepository
	{
		get
		{
			if (dealRepository == null)
			{
				dealRepository = new GenericRepository<Deal>(_pelicanContext);
			}
			return dealRepository;
		}
	}

	public IGenericRepository<Location> LocationRepository
	{
		get
		{
			if (locationRepository == null)
			{
				locationRepository = new GenericRepository<Location>(_pelicanContext);
			}
			return locationRepository;
		}
	}

	public IGenericRepository<Supplier> SupplierRepository
	{
		get
		{
			if (supplierRepository == null)
			{
				supplierRepository = new GenericRepository<Supplier>(_pelicanContext);
			}
			return supplierRepository;
		}
	}

	public UnitOfWork(IPelicanContext pelicanContext)
	{
		_pelicanContext = pelicanContext;
	}
	public void Save()
	{
		_pelicanContext.SaveChanges();
	}

	public async Task SaveAsync()
	{
		await _pelicanContext.SaveChangesAsync();
	}
}
