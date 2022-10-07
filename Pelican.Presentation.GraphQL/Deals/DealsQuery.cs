using MediatR;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Deals;
[ExtendObjectType("Query")]
public class DealsQuery
{
	public async Task<IQueryable<Deal>> GetDeals([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetDealsQuery(), cancellationToken);
	}

	public async Task<Deal> GetDealAsync(GetDealByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
