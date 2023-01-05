namespace Pelican.Presentation.GraphQL.Users;
[ExtendObjectType("Query")]
public class UsersQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<UserDto>> GetUsersAsync([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetUsersQuery(), cancellationToken);
	}
}
