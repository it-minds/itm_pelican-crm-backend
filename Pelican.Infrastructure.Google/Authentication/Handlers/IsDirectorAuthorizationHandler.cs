﻿using Microsoft.AspNetCore.Authorization;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Google.Authentication.Claims;
using Pelican.Infrastructure.Google.Authentication.Requirements;

namespace Pelican.Infrastructure.Google.Authentication.Handlers;

public class IsDirectorAuthorizationHandler : AuthorizationHandler<IsDirectorRequirement, AccountManager>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDirectorRequirement requirement, AccountManager resource)
	{
		var isDirectorClaim = context.User.FindFirst(CustomClaims.IsDirector);
		if (isDirectorClaim != null) context.Succeed(requirement);

		return Task.CompletedTask;
	}
}
