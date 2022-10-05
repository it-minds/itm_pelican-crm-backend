using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
	public ClientRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
