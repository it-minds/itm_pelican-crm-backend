using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
	private readonly IPelicanContext _pelicanContext;
	private IGenericRepository<AccountManagerDeal> accountManagerDealRepository;
	private IGenericRepository<AccountManager> accountManagerRepository;
	private IGenericRepository<Client> clientRepository;
	private IGenericRepository<Contact> contactRepository;
	private IGenericRepository<Deal> dealRepository;
	private IGenericRepository<Location> locationRepository;
	private IGenericRepository<Supplier> supplierRepository;
	private IGenericRepository<ClientContact> clientContactRepository;
	private IGenericRepository<DealContact> dealContactRepository;
	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository
	{
		get
		{
			_accountManagerDealRepository ??= new GenericRepository<AccountManagerDeal>(_pelicanContext);
			return _accountManagerDealRepository;
		}
	}

	public IGenericRepository<AccountManager> AccountManagerRepository
	{
		get
		{
			_accountManagerRepository ??= new GenericRepository<AccountManager>(_pelicanContext);
			return _accountManagerRepository;
		}
	}

	public IGenericRepository<Client> ClientRepository
	{
		get
		{
			_clientRepository ??= new GenericRepository<Client>(_pelicanContext);
			return _clientRepository;
		}
	}

	public IGenericRepository<Contact> ContactRepository
	{
		get
		{
			_contactRepository ??= new GenericRepository<Contact>(_pelicanContext);
			return _contactRepository;
		}
	}

	public IGenericRepository<Deal> DealRepository
	{
		get
		{
			_dealRepository ??= new GenericRepository<Deal>(_pelicanContext);
			return _dealRepository;
		}
	}

	public IGenericRepository<Location> LocationRepository
	{
		get
		{
			_locationRepository ??= new GenericRepository<Location>(_pelicanContext);
			return _locationRepository;
		}
	}

	public IGenericRepository<Supplier> SupplierRepository
	{
		get
		{
			_supplierRepository ??= new GenericRepository<Supplier>(_pelicanContext);
			return _supplierRepository;
		}
	}

	public IGenericRepository<ClientContact> ClientContactRepository
	{
		get
		{
			_clientContactRepository ??= new GenericRepository<ClientContact>(_pelicanContext);
			return _clientContactRepository;
		}
	}
	public IGenericRepository<ClientContact> ClientContactRepository
	{
		get
		{
			clientContactRepository ??= new GenericRepository<ClientContact>(_pelicanContext);
			return clientContactRepository;
		}
	}
	public IGenericRepository<DealContact> DealContactRepository
	{
		get
		{
			dealContactRepository ??= new GenericRepository<DealContact>(_pelicanContext);
			return dealContactRepository;
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
