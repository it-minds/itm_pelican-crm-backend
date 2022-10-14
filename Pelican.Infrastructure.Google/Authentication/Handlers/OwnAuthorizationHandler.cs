using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Google.Authentication.Claims;
using Pelican.Infrastructure.Google.Authentication.Requirements;

namespace Pelican.Infrastructure.Google.Authentication.Handlers;
public class OwnAuthorizationHandler : AuthorizationHandler<IsDirectorRequirement, AccountManager>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDirectorRequirement requirement, AccountManager resource)
	{
		if (context.HasSucceeded) return Task.CompletedTask;

		Claim idClaim = context.User.FindFirst(CustomClaims.EmployeeId);
		if (idClaim != null && resource.Id.ToString() == idClaim.Value)
			context.Succeed(requirement);

		return Task.CompletedTask;
	}
}
