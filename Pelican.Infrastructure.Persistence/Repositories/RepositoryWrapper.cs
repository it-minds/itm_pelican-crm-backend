using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class RepositoryWrapper : IRepositoryWrapper
{
	private PelicanContext _pelicaContext;
	private IAccountManagerDealRepository _accountManagerDeal;
	private IAccountManagerRepository _accountManager;
	private IClientContactRepository _clientContact;
	private IClientRepository _client;
	private IContactRepository _contact;
	private IDealContactRepository _dealContact;
	private IDealRepository _deal;
	private ILocationRepository _location;
	private ISupplierRepository _supplier;
	public IAccountManagerDealRepository AccountManagerDeal
	{
		get
		{
			if (_accountManagerDeal == null)
			{
				_accountManagerDeal = new AccountManagerDealRepository(_pelicaContext);
			}
			return _accountManagerDeal;
		}
	}

	public IAccountManagerRepository AccountManager
	{
		get
		{
			if (_accountManager == null)
			{
				_accountManager = new AccountManagerRepository(_pelicaContext);
			}
			return _accountManager;
		}
	}

	public IClientContactRepository ClientContact
	{
		get
		{
			if (_clientContact == null)
			{
				_clientContact = new ClientContactRepository(_pelicaContext);
			}
			return _clientContact;
		}
	}

	public IClientRepository Client
	{
		get
		{
			if (_client == null)
			{
				_client = new ClientRepository(_pelicaContext);
			}
			return _client;
		}
	}

	public IContactRepository Contact
	{
		get
		{
			if (_contact == null)
			{
				_contact = new ContactRepository(_pelicaContext);
			}
			return _contact;
		}
	}

	public IDealContactRepository DealContact
	{
		get
		{
			if (_dealContact == null)
			{
				_dealContact = new DealContactRepository(_pelicaContext);
			}
			return _dealContact;
		}
	}

	public IDealRepository Deal
	{
		get
		{
			if (_deal == null)
			{
				_deal = new DealRepository(_pelicaContext);
			}
			return _deal;
		}
	}

	public ILocationRepository Location
	{
		get
		{
			if (_location == null)
			{
				_location = new LocationRepository(_pelicaContext);
			}
			return _location;
		}
	}

	public ISupplierRepository Supplier
	{
		get
		{
			if (_supplier == null)
			{
				_supplier = new SupplierRepository(_pelicaContext);
			}
			return _supplier;
		}
	}
	public RepositoryWrapper(PelicanContext pelicanContext)
	{
		_pelicaContext = pelicanContext;
	}
	public void Save()
	{
		_pelicaContext.SaveChanges();
	}
}
