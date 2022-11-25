using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocations;
public record GetLocationsQuery() : IQuery<IQueryable<Location>>;
