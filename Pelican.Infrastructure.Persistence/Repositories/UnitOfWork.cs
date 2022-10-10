using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
	private IPelicanContext _pelicanContext;
	private IAccountManagerDealRepository? _accountManagerDealRepository;
	private IAccountManagerRepository? _accountManagerRepository;
	private IClientContactRepository? _clientContactRepository;
	private IClientRepository? _clientRepository;
	private IContactRepository? _contactRepository;
	private IDealContactRepository? _dealContactRepository;
	private IDealRepository? _dealRepository;
	private ILocationRepository? _locationRepository;
	private ISupplierRepository? _supplierRepository;
	public IAccountManagerDealRepository AccountManagerDealRepository
	{
		get
		{
			if (_accountManagerDealRepository == null)
			{
				_accountManagerDealRepository = new AccountManagerDealRepository(_pelicanContext);
			}
			return _accountManagerDealRepository;
		}
	}

	public IAccountManagerRepository AccountManagerRepository
	{
		get
		{
			if (_accountManagerRepository == null)
			{
				_accountManagerRepository = new AccountManagerRepository(_pelicanContext);
			}
			return _accountManagerRepository;
		}
	}

	public IClientContactRepository ClientContactRepository
	{
		get
		{
			if (_clientContactRepository == null)
			{
				_clientContactRepository = new ClientContactRepository(_pelicanContext);
			}
			return _clientContactRepository;
		}
	}

	public IClientRepository ClientRepository
	{
		get
		{
			if (_clientRepository == null)
			{
				_clientRepository = new ClientRepository(_pelicanContext);
			}
			return _clientRepository;
		}
	}

	public IContactRepository ContactRepository
	{
		get
		{
			if (_contactRepository == null)
			{
				_contactRepository = new ContactRepository(_pelicanContext);
			}
			return _contactRepository;
		}
	}

	public IDealContactRepository DealContactRepository
	{
		get
		{
			if (_dealContactRepository == null)
			{
				_dealContactRepository = new DealContactRepository(_pelicanContext);
			}
			return _dealContactRepository;
		}
	}

	public IDealRepository DealRepository
	{
		get
		{
			if (_dealRepository == null)
			{
				_dealRepository = new DealRepository(_pelicanContext);
			}
			return _dealRepository;
		}
	}

	public ILocationRepository LocationRepository
	{
		get
		{
			if (_locationRepository == null)
			{
				_locationRepository = new LocationRepository(_pelicanContext);
			}
			return _locationRepository;
		}
	}

	public ISupplierRepository SupplierRepository
	{
		get
		{
			if (_supplierRepository == null)
			{
				_supplierRepository = new SupplierRepository(_pelicanContext);
			}
			return _supplierRepository;
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
