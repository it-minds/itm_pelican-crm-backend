using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ClientContactRepository : RepositoryBase<ClientContact>, IClientContactRepository
{
	public ClientContactRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
