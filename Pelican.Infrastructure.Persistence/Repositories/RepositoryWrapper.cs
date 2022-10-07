using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class RepositoryWrapper : IRepositoryWrapper
{
	private IPelicanContext _pelicanContext;
	private IAccountManagerDealRepository? _accountManagerDeal;
	private IAccountManagerRepository? _accountManager;
	private IClientContactRepository? _clientContact;
	private IClientRepository? _client;
	private IContactRepository? _contact;
	private IDealContactRepository? _dealContact;
	private IDealRepository? _deal;
	private ILocationRepository? _location;
	private ISupplierRepository? _supplier;
	public IAccountManagerDealRepository AccountManagerDeal
	{
		get
		{
			if (_accountManagerDeal == null)
			{
				_accountManagerDeal = new AccountManagerDealRepository(_pelicanContext);
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
				_accountManager = new AccountManagerRepository(_pelicanContext);
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
				_clientContact = new ClientContactRepository(_pelicanContext);
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
				_client = new ClientRepository(_pelicanContext);
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
				_contact = new ContactRepository(_pelicanContext);
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
				_dealContact = new DealContactRepository(_pelicanContext);
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
				_deal = new DealRepository(_pelicanContext);
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
				_location = new LocationRepository(_pelicanContext);
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
				_supplier = new SupplierRepository(_pelicanContext);
			}
			return _supplier;
		}
	}
	public RepositoryWrapper(IPelicanContext pelicanContext)
	{
		_pelicanContext = pelicanContext;
	}
	public void Save()
	{
		_pelicanContext.SaveChanges();
	}
}
