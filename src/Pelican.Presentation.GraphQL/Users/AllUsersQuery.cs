using MediatR;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Queries.GetAllUsers;

namespace Pelican.Presentation.GraphQL.Users;
[ExtendObjectType("Query")]
public class AllUsersQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<UserDto>> GetUsersAsync([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetAllUsersQuery(), cancellationToken);
	}
}
