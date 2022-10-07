using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDeals;
public record GetDealsQuery() : IRequest<IQueryable<Deal>>;
