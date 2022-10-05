using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class DealRepository : RepositoryBase<Deal>, IDealRepository
{
	public DealRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
