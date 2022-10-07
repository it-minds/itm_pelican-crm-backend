namespace Pelican.Domain.Repositories;
public interface IUnitOfWork
{
	IAccountManagerDealRepository AccountManagerDealRepository { get; }
	IAccountManagerRepository AccountManagerRepository { get; }
	IClientContactRepository ClientContactRepository { get; }
	IClientRepository ClientRepository { get; }
	IContactRepository ContactRepository { get; }
	IDealContactRepository DealContactRepository { get; }
	IDealRepository DealRepository { get; }
	ILocationRepository LocationRepository { get; }
	ISupplierRepository SupplierRepository { get; }
	void Save();
	void SaveAsync();
}
