using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetCLients;
public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, IQueryable<Client>>
{
	private readonly IGenericRepository<Client> _repository;
	public GetClientsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.ClientRepository;
	}
	//Uses the repository for Client to find all Clients in the database
	public async Task<IQueryable<Client>> Handle(GetClientsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAllWithIncludes();//.Include(client => client.ClientContacts);//.ThenInclude(clientContact=>clientContact.Contact).ThenInclude(contact=>contact.DealContacts);

	}
}
