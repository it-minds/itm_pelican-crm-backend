namespace Pelican.Application.Users.Queries.GetUsers;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
public record GetUsersQuery() : IQuery<IQueryable<UserDto>>;

