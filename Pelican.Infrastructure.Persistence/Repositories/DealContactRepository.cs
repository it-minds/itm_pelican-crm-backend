using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
internal class DealContactRepository : RepositoryBase<DealContact>, IDealContactRepository
{
	public DealContactRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
