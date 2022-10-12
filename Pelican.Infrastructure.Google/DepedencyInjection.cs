using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pelican.Infrastructure.Google;
public static class DepedencyInjection
{
	public static IServiceCollection AddGoogleAuth(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

		}).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
		{
			options.ClientId = configuration["Authentication:Google:ClientId"];
			options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
			options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
		});
		return services;
	}
}
