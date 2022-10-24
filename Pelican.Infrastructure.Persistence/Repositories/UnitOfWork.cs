using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Primitives;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
	private readonly IPelicanContext _pelicanContext;
	private IGenericRepository<AccountManagerDeal> _accountManagerDealRepository;
	private IGenericRepository<AccountManager> _accountManagerRepository;
	private IGenericRepository<Client> _clientRepository;
	private IGenericRepository<Contact> _contactRepository;
	private IGenericRepository<Deal> _dealRepository;
	private IGenericRepository<Location> _locationRepository;
	private IGenericRepository<Supplier> _supplierRepository;
	private IGenericRepository<ClientContact> _clientContactRepository;
	private IGenericRepository<DealContact> _dealContactRepository;

	public IGenericRepository<T> GetRepository<T>() where T : Entity
	{
		if (typeof(T) == typeof(AccountManager))
		{
			return (IGenericRepository<T>)AccountManagerRepository;
		}
		if (typeof(T) == typeof(AccountManagerDeal))
		{
			return (IGenericRepository<T>)AccountManagerDealRepository;
		}
		if (typeof(T) == typeof(Client))
		{
			return (IGenericRepository<T>)ClientRepository;
		}
		if (typeof(T) == typeof(ClientContact))
		{
			return (IGenericRepository<T>)ClientContactRepository;
		}
		if (typeof(T) == typeof(Contact))
		{
			return (IGenericRepository<T>)ContactRepository;
		}
		if (typeof(T) == typeof(Deal))
		{
			return (IGenericRepository<T>)DealRepository;
		}
		if (typeof(T) == typeof(DealContact))
		{
			return (IGenericRepository<T>)DealContactRepository;
		}
		if (typeof(T) == typeof(Location))
		{
			return (IGenericRepository<T>)LocationRepository;
		}
		if (typeof(T) == typeof(Supplier))
		{
			return (IGenericRepository<T>)SupplierRepository;
		}
		throw new ArgumentException("Repository is not of valid Entity type");

	}
	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository
	{
		get
		{
			_accountManagerDealRepository ??= new AccountManagerDealsRepository(_pelicanContext);
			return _accountManagerDealRepository;
		}
	}

	public IGenericRepository<AccountManager> AccountManagerRepository
	{
		get
		{
			_accountManagerRepository ??= new AccountManagerRepository(_pelicanContext);
			return _accountManagerRepository;
		}
	}

	public IGenericRepository<Client> ClientRepository
	{
		get
		{
			_clientRepository ??= new ClientRepository(_pelicanContext);
			return _clientRepository;
		}
	}

	public IGenericRepository<Contact> ContactRepository
	{
		get
		{
			_contactRepository ??= new ContactRepository(_pelicanContext);
			return _contactRepository;
		}
	}

	public IGenericRepository<Deal> DealRepository
	{
		get
		{
			_dealRepository ??= new DealRepository(_pelicanContext);
			return _dealRepository;
		}
	}

	public IGenericRepository<Location> LocationRepository
	{
		get
		{
			_locationRepository ??= new LocationRepository(_pelicanContext);
			return _locationRepository;
		}
	}

	public IGenericRepository<Supplier> SupplierRepository
	{
		get
		{
			_supplierRepository ??= new SupplierRepository(_pelicanContext);
			return _supplierRepository;
		}
	}

	public IGenericRepository<ClientContact> ClientContactRepository
	{
		get
		{
			_clientContactRepository ??= new ClientContactsRepository(_pelicanContext);
			return _clientContactRepository;
		}
	}

	public IGenericRepository<DealContact> DealContactRepository
	{
		get
		{
			_dealContactRepository ??= new DealContactsRepository(_pelicanContext);
			return _dealContactRepository;
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

	public async Task SaveAsync(CancellationToken cancellationToken)
	{
		await _pelicanContext.SaveChangesAsync(cancellationToken);
	}
}
