using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
	public ContactRepository(PelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
