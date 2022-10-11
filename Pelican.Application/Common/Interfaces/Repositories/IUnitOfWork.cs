using Pelican.Domain.Entities;

namespace Pelican.Domain.Repositories;
public interface IUnitOfWork
{
	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository { get; }
	public IGenericRepository<AccountManager> AccountManagerRepository { get; }
	public IGenericRepository<Client> ClientRepository { get; }
	public IGenericRepository<Contact> ContactRepository { get; }
	public IGenericRepository<Deal> DealRepository { get; }
	public IGenericRepository<Location> LocationRepository { get; }
	public IGenericRepository<Supplier> SupplierRepository { get; }
	void Save();
	Task SaveAsync();
}
