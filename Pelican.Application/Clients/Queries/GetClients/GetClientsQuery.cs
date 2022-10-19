using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClients;
public record GetClientsQuery() : IQuery<IQueryable<Client>>;
