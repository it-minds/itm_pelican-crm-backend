using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContactById;
public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, Contact>
{
	private readonly IContactByIdDataLoader _dataLoader;
	public GetContactByIdQueryHandler(IContactByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Contact in the database using their Id
	public async Task<Contact> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
