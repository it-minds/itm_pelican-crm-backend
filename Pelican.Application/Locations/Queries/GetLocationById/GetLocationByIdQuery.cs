using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocationById;
public record GetLocationByIdQuery([ID(nameof(Location))] Guid Id) : IRequest<Location>;
