using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClientById;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
public record GetClientByIdQuery([ID(nameof(Client))] Guid Id) : IQuery<Client>;
