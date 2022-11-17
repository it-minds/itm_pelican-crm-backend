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

	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository => _accountManagerDealRepository ??= new GenericRepository<AccountManagerDeal>(_pelicanContext);

	public IGenericRepository<AccountManager> AccountManagerRepository => _accountManagerRepository ??= new GenericRepository<AccountManager>(_pelicanContext);

	public IGenericRepository<Client> ClientRepository => _clientRepository ??= new GenericRepository<Client>(_pelicanContext);

	public IGenericRepository<Contact> ContactRepository => _contactRepository ??= new GenericRepository<Contact>(_pelicanContext);

	public IGenericRepository<Deal> DealRepository => _dealRepository ??= new GenericRepository<Deal>(_pelicanContext);

	public IGenericRepository<Location> LocationRepository => _locationRepository ??= new GenericRepository<Location>(_pelicanContext);

	public IGenericRepository<Supplier> SupplierRepository => _supplierRepository ??= new GenericRepository<Supplier>(_pelicanContext);

	public IGenericRepository<ClientContact> ClientContactRepository => _clientContactRepository ??= new GenericRepository<ClientContact>(_pelicanContext);

	public IGenericRepository<DealContact> DealContactRepository => _dealContactRepository ??= new GenericRepository<DealContact>(_pelicanContext);

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
