using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContacts;
public record GetContactsQuery() : IQuery<IQueryable<Contact>>;
