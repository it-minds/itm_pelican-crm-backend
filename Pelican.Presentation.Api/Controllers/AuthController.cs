using Google.Apis.Auth.AspNetCore3;
using LazyCache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pelican.Domain.Enums;

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
		return Redirect(returnUrl);
	}
	[Authorize()]
	[HttpGet("sign-out")]
	public async Task<IActionResult> AppSignOut()
	{
		await HttpContext.SignOutAsync();
		return Ok();
	}
}
