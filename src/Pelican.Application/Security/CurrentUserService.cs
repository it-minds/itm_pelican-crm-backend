

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pelican.Application.Abstractions.Authentication;

namespace Pelican.Application.Security;

public class CurrentUserService : ICurrentUserService
{

	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? UserId => _httpContextAccessor
		.HttpContext?
		.User?
		.FindFirst(ClaimTypes.NameIdentifier)?
		.Value;
}
