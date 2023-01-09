using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Queries.GetAdmins;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
public record GetAdminsQuery() : IQuery<IQueryable<UserDto>>;
