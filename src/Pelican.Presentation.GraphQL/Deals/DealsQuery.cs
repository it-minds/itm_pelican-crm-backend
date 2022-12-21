using MediatR;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Presentation.GraphQL.Deals;

[Authorize(Role = RoleEnum.Standard)]
[Authorize(Role = RoleEnum.Admin)]
[ExtendObjectType("Query")]
public class DealsQuery
{
	//This Query reguests all Deals from the database.
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<Deal>> GetDeals([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetDealsQuery(), cancellationToken);
	}
	//This Query reguests a specific Deal from the database.
	public async Task<Deal> GetDealAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetDealByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}
