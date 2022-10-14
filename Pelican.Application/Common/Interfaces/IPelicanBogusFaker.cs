using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces;
public interface IPelicanBogusFaker
{
	IEnumerable<Supplier> SupplierFaker(int count);
	IEnumerable<AccountManager> AccountManagerFaker(int supplierCount, IQueryable<Supplier> suppliers);
}
