using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContactById;
public class GetContactByIdQueryHandler : IQueryHandler<GetContactByIdQuery, Contact>
{
	private readonly IGenericDataLoader<Contact> _dataLoader;
	public GetContactByIdQueryHandler(IGenericDataLoader<Contact> dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Contact in the database using their Id
	public async Task<Contact> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
