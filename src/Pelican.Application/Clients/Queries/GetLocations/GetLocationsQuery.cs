using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetLocations;

public record GetLocationsQuery() : IQuery<IQueryable<Client>>;
