using Pelican.Domain.Entities;

namespace Pelican.Application.Abstractions.Data;
public interface IPelicanBogusFaker
{
	public IEnumerable<AccountManager> AccountManagerFaker(int supplierCount, IQueryable<Supplier> suppliers);
	public IEnumerable<Supplier> SupplierFaker(int count);
	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients, IQueryable<AccountManager> accountManagers);
	public IEnumerable<Location> LocationFaker(int count, IQueryable<Supplier> suppliers);
	public IEnumerable<Client> ClientFaker(int count, IQueryable<Location> locations);
	public IEnumerable<Contact> ContactFaker(int count, IQueryable<AccountManager> accountManagers);
	public IEnumerable<AccountManagerDeal> AccountManagerDealFaker(IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals);
	public IEnumerable<DealContact> DealContactFaker(IQueryable<Deal> deals, IQueryable<Contact> contacts);
	public IEnumerable<ClientContact> ClientContactFaker(IQueryable<Client> clients, IQueryable<Contact> contacts);
}
