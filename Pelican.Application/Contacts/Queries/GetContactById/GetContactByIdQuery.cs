using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContactById;
public record GetContactByIdQuery([ID(nameof(Contact))] Guid Id) : IRequest<Contact>;
