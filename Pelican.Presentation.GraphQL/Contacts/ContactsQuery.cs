using MediatR;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Contacts;

[ExtendObjectType(OperationTypeNames.Query)]
public class ContactsQuery
{
	public async Task<IQueryable<Contact>> GetContacts([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetContactsQuery(), cancellationToken);
	}

	public async Task<Contact> GetContactAsync(GetContactByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
