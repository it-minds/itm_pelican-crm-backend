using Pelican.Domain.Entities;

namespace Pelican.Domain.Repositories;
public interface IUnitOfWork
{
	public IRepositoryBase<AccountManagerDeal> AccountManagerDealRepository { get; }
	public IRepositoryBase<AccountManager> AccountManagerRepository { get; }
	public IRepositoryBase<Client> ClientRepository { get; }
	public IRepositoryBase<Contact> ContactRepository { get; }
	public IRepositoryBase<Deal> DealRepository { get; }
	public IRepositoryBase<Location> LocationRepository { get; }
	public IRepositoryBase<Supplier> SupplierRepository { get; }
	void Save();
	Task SaveAsync();
}
