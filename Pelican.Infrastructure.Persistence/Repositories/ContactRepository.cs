using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
	public ContactRepository(IPelicanContext pelicanContext) : base(pelicanContext)
	{
	}
}
