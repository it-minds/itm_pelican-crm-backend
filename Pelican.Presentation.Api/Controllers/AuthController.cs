using Google.Apis.Auth.AspNetCore3;
using LazyCache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pelican.Domain.Enums;
using Pelican.Infrastructure.Google.Authentication;
using Pelican.Infrastructure.Google.Authentication.Claims;

namespace Pelican.Presentation.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
	private readonly IAppCache _cache;
	public AuthController(IAppCache cache)
	{
		_cache = cache;
	}

	[Authorize(AuthenticationSchemes = GoogleOpenIdConnectDefaults.AuthenticationScheme)]
	[HttpGet("sign-in")]
	public IActionResult SignIn(Country country, string returnUrl, string errorUrl)
	{
		var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GoogleClaims.Email);
		if (emailClaim != null)
		{
			var groupCacheKey = GoogleGroups.GetGroupCacheStringForUser(emailClaim.Value);
			_cache.Remove(groupCacheKey);
		}

		return Redirect(returnUrl);
	}
	[Authorize()]
	[HttpGet("sign-out")]
	public async Task<IActionResult> AppSignOut()
	{
		var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GoogleClaims.Email);
		if (emailClaim != null)
		{
			var groupCacheKey = GoogleGroups.GetGroupCacheStringForUser(emailClaim.Value);
			_cache.Remove(groupCacheKey);
		}
		await HttpContext.SignOutAsync();
		return Ok();
	}
}
