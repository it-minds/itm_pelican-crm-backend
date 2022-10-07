using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class DealContactRepository : RepositoryBase<DealContact>, IDealContactRepository
{
	public DealContactRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
