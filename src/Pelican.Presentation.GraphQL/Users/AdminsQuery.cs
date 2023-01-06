using MediatR;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Queries.GetAdmins;

namespace Pelican.Presentation.GraphQL.Users;
[ExtendObjectType("Query")]
public class AdminsQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<UserDto>> GetAdminsAsync([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetAdminsQuery(), cancellationToken);
	}
}
