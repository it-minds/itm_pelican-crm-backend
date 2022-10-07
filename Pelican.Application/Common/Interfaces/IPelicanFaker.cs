using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces;
public interface IPelicanFaker
{
	IEnumerable<Supplier> SupplierFaker(int count);
}
