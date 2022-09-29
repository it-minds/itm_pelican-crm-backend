using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class SupplierRepository
{
	private readonly PelicanContext _dbcontext;
	public SupplierRepository(PelicanContext dbcontext)
	{
		_dbcontext = dbcontext;
	}
	public List<Supplier> GetSuppliers()
	{
		return _dbcontext.Suppliers.ToList<Supplier>();
	}
	public Supplier? GetSupplierById(Guid id)
	{
		return _dbcontext.Suppliers.FirstOrDefault(s => s.Id == id);
	}
	public async Task<Supplier> CreateAsync(Supplier supplier)
	{
		await _dbcontext.Suppliers.AddAsync(supplier);
		await _dbcontext.SaveChangesAsync();
		return supplier;
	}
	public async Task<Supplier> updateAsync(Supplier supplier)
	{
		_dbcontext.Suppliers.Update(supplier);
		await _dbcontext.SaveChangesAsync();
		return supplier;
	}
	public
}
