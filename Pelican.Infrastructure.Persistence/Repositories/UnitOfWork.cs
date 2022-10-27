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

	public IGenericRepository<T> GetRepository<T>() where T : Entity => typeof(T) switch
	{
		Type AccountManager when AccountManager == typeof(AccountManager)
			=> (IGenericRepository<T>)AccountManagerRepository,
		Type AccountManagerDeal when AccountManagerDeal == typeof(AccountManagerDeal)
			=> (IGenericRepository<T>)AccountManagerDealRepository,
		Type ClientContacts when ClientContacts == typeof(ClientContact)
			=> (IGenericRepository<T>)ClientContactRepository,
		Type Client when Client == typeof(Client)
			=> (IGenericRepository<T>)ClientRepository,
		Type Contact when Contact == typeof(Contact)
			=> (IGenericRepository<T>)ContactRepository,
		Type DealContact when DealContact == typeof(DealContact)
			=> (IGenericRepository<T>)DealContactRepository,
		Type Deal when Deal == typeof(Deal)
			=> (IGenericRepository<T>)DealRepository,
		Type Location when Location == typeof(Location)
			=> (IGenericRepository<T>)LocationRepository,
		Type Supplier when Supplier == typeof(Supplier)
			=> (IGenericRepository<T>)SupplierRepository,
		_ => throw new ArgumentException("Generic Repository is not of correct Entity type", nameof(T)),
	};
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
