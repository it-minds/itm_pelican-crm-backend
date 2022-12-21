using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Contacts.Queries.GetContactById;
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetContactByIdQuery([ID(nameof(Contact))] Guid Id) : IQuery<Contact>;
