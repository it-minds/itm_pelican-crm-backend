using MediatR;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Clients.Queries.GetCLients;
public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IQueryable<Client>>
{
	private readonly IGenericRepository<Client> _repository;
	public GetClientsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.ClientRepository;
	}
	//Uses the repository for Client to find all Clients in the database
	public async Task<IQueryable<Client>> Handle(GetClientsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
