using Pelican.Application.Abstractions.Authentication;
using Pelican.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Pelican.Application.Security;


public class AuthorizationService : IAuthorizationService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AuthorizationService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public bool IsInRole(RoleEnum role) => role
		.ToString()
		.Equals(_httpContextAccessor
			.HttpContext?
			.User?
			.FindFirst(ClaimTypes.Role));
}
