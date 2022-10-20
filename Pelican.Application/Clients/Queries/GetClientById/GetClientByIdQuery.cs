using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClientById;
public record GetClientByIdQuery([ID(nameof(Client))] Guid Id) : IQuery<Client>;
