using MediatR;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Contacts;

[ExtendObjectType(OperationTypeNames.Query)]
public class ContactsQuery
{
	//This Query reguests all contacts from the database.
	[UsePaging]
	public async Task<IQueryable<Contact>> GetContacts([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetContactsQuery(), cancellationToken);
	}
	//This Query reguests a specific Contact from the database.
	public async Task<Contact> GetContactAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetContactByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}
