using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;


namespace Pelican.Application.Common.Interfaces;

public interface IPelicanContext : IDbContext
{
	DbSet<AccountManager> AccountManagers { get; }
	DbSet<AccountManagerDeal> AccountManagerDeals { get; }
	DbSet<Client> Clients { get; }
	DbSet<ClientContact> ClientContacts { get; }
	DbSet<Contact> Contacts { get; }
	DbSet<Deal> Deals { get; }
	DbSet<DealContact> DealContacs { get; }
	DbSet<Supplier> Suppliers { get; }
	DbSet<Location> Locations { get; }
	string DbPath { get; }
	int SaveChanges();
	int SaveChanges(bool acceptAllChangesOnSuccess);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

	void MarkAsModifiedAccountManager(AccountManager accountManager);
	void MarkAsModifiedAccountManagerDeals(AccountManagerDeal accountManagerDeal);
	void MarkAsModifiedClient(Client client);
	void MarkAsModifiedClientContact(ClientContact clientContact);
	void MarkAsModifiedContact(Contact contact);
	void MarkAsModifiedDeals(Deal deal);
	void MarkAsModifiedDealContact(DealContact dealContact);
	void MarkAsModifiedClientSupplier(Supplier supplier);
}
