using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Queries.GetAllUsers;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
public record GetAllUsersQuery() : IQuery<IQueryable<UserDto>>;

