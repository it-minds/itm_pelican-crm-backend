using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClients;
public record GetClientsQuery() : IRequest<IQueryable<Client>>;
