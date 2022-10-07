using MediatR;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.AccountManagers;
[ExtendObjectType("Query")]
public class AccountManagersQuery
{
	public async Task<IQueryable<AccountManager>> GetAccountManagers([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetAccountManagersQuery(), cancellationToken);
	}

	public async Task<AccountManager> GetAccountManagerAsync(GetAccountManagerByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
