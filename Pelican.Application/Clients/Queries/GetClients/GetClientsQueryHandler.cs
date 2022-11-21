using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClients;
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
		return await Task.Run(() => _repository.FindAll());
	}
}
