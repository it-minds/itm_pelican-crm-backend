using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContactById;
public record GetContactByIdQuery([ID(nameof(Contact))] Guid Id) : IQuery<Contact>;
