using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
	private IPelicanContext _pelicanContext;
	private RepositoryBase<AccountManagerDeal> accountManagerDealRepository;
	private RepositoryBase<AccountManager> accountManagerRepository;
	private RepositoryBase<Client> clientRepository;
	private RepositoryBase<Contact> contactRepository;
	private RepositoryBase<Deal> dealRepository;
	private RepositoryBase<Location> locationRepository;
	private RepositoryBase<Supplier> supplierRepository;


	public IRepositoryBase<AccountManagerDeal> AccountManagerDealRepository
	{
		get
		{
			if (accountManagerDealRepository == null)
			{
				accountManagerDealRepository = new RepositoryBase<AccountManagerDeal>(_pelicanContext);
			}
			return accountManagerDealRepository;
		}
	}

	public IRepositoryBase<AccountManager> AccountManagerRepository
	{
		get
		{
			if (accountManagerRepository == null)
			{
				accountManagerRepository = new RepositoryBase<AccountManager>(_pelicanContext);
			}
			return accountManagerRepository;
		}
	}

	public IRepositoryBase<Client> ClientRepository
	{
		get
		{
			if (clientRepository == null)
			{
				clientRepository = new RepositoryBase<Client>(_pelicanContext);
			}
			return clientRepository;
		}
	}

	public IRepositoryBase<Contact> ContactRepository
	{
		get
		{
			if (contactRepository == null)
			{
				contactRepository = new RepositoryBase<Contact>(_pelicanContext);
			}
			return contactRepository;
		}
	}

	public IRepositoryBase<Deal> DealRepository
	{
		get
		{
			if (dealRepository == null)
			{
				dealRepository = new RepositoryBase<Deal>(_pelicanContext);
			}
			return dealRepository;
		}
	}

	public IRepositoryBase<Location> LocationRepository
	{
		get
		{
			if (locationRepository == null)
			{
				locationRepository = new RepositoryBase<Location>(_pelicanContext);
			}
			return locationRepository;
		}
	}

	public IRepositoryBase<Supplier> SupplierRepository
	{
		get
		{
			if (supplierRepository == null)
			{
				supplierRepository = new RepositoryBase<Supplier>(_pelicanContext);
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
