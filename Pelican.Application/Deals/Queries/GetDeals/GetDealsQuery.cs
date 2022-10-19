using MediatR;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDeals;
public record GetDealsQuery() : IQuery<IQueryable<Deal>>;
