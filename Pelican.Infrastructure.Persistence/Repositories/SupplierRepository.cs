using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
{
	public SupplierRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}

}
