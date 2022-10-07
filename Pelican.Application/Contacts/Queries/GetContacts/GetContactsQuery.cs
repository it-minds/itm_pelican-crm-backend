using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContacts;
public record GetContactsQuery() : IRequest<IQueryable<Contact>>;
