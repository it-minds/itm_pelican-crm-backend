using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Infrastructure.Google.Authentication;
using Pelican.Infrastructure.Google.Authentication.Interfaces;
using Pelican.Infrastructure.Google.Authentication.Requirements;
using Pelican.Infrastructure.Google.Authentication.Services;

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
		services.AddAuthorization(options =>
		{
			options.AddPolicy(AccessPoliciesStrings.Own, policy => policy.AddRequirements(new OwnRequirement()));
		});
		services.AddSingleton<IClaimsTransformation, GoogleClaimsTransformation>();
		services.AddSingleton<IEmployeeClaimsService, EmployeeClaimsService>();
		services.AddLazyCache();

		return services;
	}
}
