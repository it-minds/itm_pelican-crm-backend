using Pelican.Domain.Entities;
using Pelican.Domain.Primitives;

namespace Pelican.Application.Common.Interfaces.Repositories;
public interface IUnitOfWork
{
	public IGenericRepository<AccountManagerDeal> AccountManagerDealRepository { get; }
	public IGenericRepository<AccountManager> AccountManagerRepository { get; }
	public IGenericRepository<Client> ClientRepository { get; }
	public IGenericRepository<Contact> ContactRepository { get; }
	public IGenericRepository<Deal> DealRepository { get; }
	public IGenericRepository<Location> LocationRepository { get; }
	public IGenericRepository<Supplier> SupplierRepository { get; }
	public IGenericRepository<ClientContact> ClientContactRepository { get; }
	public IGenericRepository<DealContact> DealContactRepository { get; }
	public IGenericRepository<T> GetRepository<T>() where T : Entity;
	void Save();
	Task SaveAsync(CancellationToken cancellationToken);
}
