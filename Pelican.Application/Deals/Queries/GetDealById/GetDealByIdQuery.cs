using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDealById;
public record GetDealByIdQuery([ID(nameof(Deal))] Guid Id) : IQuery<Deal>;
