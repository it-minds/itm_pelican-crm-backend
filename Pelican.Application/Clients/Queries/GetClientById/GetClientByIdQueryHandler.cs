using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClientById;
public class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, Client>
{
	private readonly IGenericDataLoader<Client> _dataLoader;
	public GetClientByIdQueryHandler(IGenericDataLoader<Client> dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Client in the database using their Id

	public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
