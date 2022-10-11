using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocations;
public record GetLocationsQuery() : IRequest<IQueryable<Location>>;
