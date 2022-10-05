using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class LocationRepository : RepositoryBase<Location>, ILocationRepository
{
	public LocationRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
