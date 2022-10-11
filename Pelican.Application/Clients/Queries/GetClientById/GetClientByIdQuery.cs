using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClientById;
public record GetClientByIdQuery([ID(nameof(Client))] Guid Id) : IRequest<Client>;
