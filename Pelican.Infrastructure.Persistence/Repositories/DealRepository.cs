using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class DealRepository : RepositoryBase<Deal>, IDealRepository
{
	public DealRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
