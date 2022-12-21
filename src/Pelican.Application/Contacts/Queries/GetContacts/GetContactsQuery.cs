using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Contacts.Queries.GetContacts;
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetContactsQuery() : IQuery<IQueryable<Contact>>;
