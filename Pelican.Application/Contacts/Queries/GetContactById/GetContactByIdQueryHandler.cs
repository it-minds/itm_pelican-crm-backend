using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContactById;
public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, Contact>
{
	private readonly IClientByIdDataLoader _dataLoader;
	public GetContactByIdQueryHandler(IClientByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
