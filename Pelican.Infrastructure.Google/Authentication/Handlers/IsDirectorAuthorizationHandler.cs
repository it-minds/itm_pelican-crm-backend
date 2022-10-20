using Microsoft.AspNetCore.Authorization;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Authentication.Authentication.Claims;
using Pelican.Infrastructure.Authentication.Authentication.Requirements;

namespace Pelican.Infrastructure.Authentication.Authentication.Handlers;

public class IsDirectorAuthorizationHandler : AuthorizationHandler<IsDirectorRequirement, AccountManager>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDirectorRequirement requirement, AccountManager resource)
	{
		var isDirectorClaim = context.User.FindFirst(CustomClaims.IsDirector);
		if (isDirectorClaim != null) context.Succeed(requirement);

		return Task.CompletedTask;
	}
}
