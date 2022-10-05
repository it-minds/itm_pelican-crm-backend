using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
{
	public SupplierRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}

}
