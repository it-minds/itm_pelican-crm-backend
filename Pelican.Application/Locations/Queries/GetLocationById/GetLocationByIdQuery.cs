using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocationById;
public record GetLocationByIdQuery([ID(nameof(Location))] Guid Id) : IQuery<Location>;
