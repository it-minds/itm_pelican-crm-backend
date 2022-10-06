using MediatR;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Clients.Queries.GetCLients;
internal class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IQueryable<Client>>
{
	private readonly IClientRepository _repository;
	public GetClientsQueryHandler(IClientRepository clientRepository)
	{
		_repository = clientRepository;
	}
	public async Task<IQueryable<Client>> Handle(GetClientsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
