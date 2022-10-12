using LazyCache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pelican.Infrastructure.Google.Authentication;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
	private readonly IAppCache _cache;
	private readonly IDbContextFactory<PelicanContext> _pelicanContext;
	public AuthController(IAppCache cache, IDbContextFactory<PelicanContext> pelicanContext)
	{
		_cache = cache;
		_pelicanContext = pelicanContext;
	}

	public async Task Login()
	{
		await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
		{
			RedirectUri = Url.Action("GoogleResponse")
		});
	}

	public async Task<IActionResult> GoogleResponse()
	{
		var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
		{
			claim.Issuer,
			claim.OriginalIssuer,
			claim.Type,
			claim.Value
		});
		return Json(claims);
	}
	public async Task LogOut()
	{
		var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GoogleClaims.Email);
		if (emailClaim != null)
		{
			var groupCacheKey = GoogleGroups.GetGroupCacheStringForUser(emailClaim.Value);

			_cache.Remove(groupCacheKey);
		}
		await HttpContext.SignOutAsync();
	}
}
