using MediatR;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Presentation.GraphQL.AccountManagers;

[Authorize(Role = RoleEnum.Standard)]
[Authorize(Role = RoleEnum.Admin)]
[ExtendObjectType("Query")]
public class AccountManagersQuery
{
	//This Query reguests all AccountManager from the database.
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<AccountManager>> GetAccountManagers([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetAccountManagersQuery(), cancellationToken);
	}
	//This Query reguests a specific AccountManager from the database.
	public async Task<AccountManager> GetAccountManagerAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetAccountManagerByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}
