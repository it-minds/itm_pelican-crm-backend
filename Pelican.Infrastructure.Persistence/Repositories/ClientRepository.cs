using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
	public ClientRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
