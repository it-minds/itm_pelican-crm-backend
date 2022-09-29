namespace Pelican.Domain.Repositories;
public interface IRepositoryWrapper
{
	IAccountManagerDealRepository AccountManagerDeal { get; }
	IAccountManagerRepository AccountManager { get; }
	IClientContactRepository ClientContact { get; }
	IClientRepository Client { get; }
	IContactRepository Contact { get; }
	IDealContactRepository DealContact { get; }
	IDealRepository Deal { get; }
	ILocationRepository Location { get; }
	ISupplierRepository Supplier { get; }
}
