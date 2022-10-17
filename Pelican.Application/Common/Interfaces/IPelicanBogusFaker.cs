﻿using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces;
public interface IPelicanBogusFaker
{
	public IEnumerable<AccountManager> AccountManagerFaker(int supplierCount, IQueryable<Supplier> suppliers);
	public IEnumerable<Supplier> SupplierFaker(int count, IQueryable<Location> locations);
	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients);
	public IEnumerable<Location> LocationFaker(int count);
	public IEnumerable<Client> ClientFaker(int count, IQueryable<Location> locations);
	public IEnumerable<Contact> ContactFaker(int count);
}
