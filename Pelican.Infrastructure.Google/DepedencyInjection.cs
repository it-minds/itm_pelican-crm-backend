using System.Web;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Pelican.Domain.Enums;
using Pelican.Infrastructure.Google.Authentication;
using Pelican.Infrastructure.Google.Authentication.Handlers;
using Pelican.Infrastructure.Google.Authentication.Requirements;

namespace Pelican.Infrastructure.Google;
public static class DepedencyInjection
{
	public static IServiceCollection AddGoogleAuth(this IServiceCollection services, IConfiguration configuration)
	{
		if (configuration.GetValue<bool>("ASPNETCORE_FORWARDEDHEADERS_ENABLED"))
		{
			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
										   ForwardedHeaders.XForwardedProto;
				// Only loopback proxies are allowed by default.
				// Clear that restriction because forwarders are enabled by explicit 
				// configuration.
				options.KnownNetworks.Clear();
				options.KnownProxies.Clear();
			});
		}
		services.AddAuthentication(options =>
		{
			options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
			options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		})
			.AddCookie(options =>
			{
				options.Cookie.Name = "Pelican.Auth";
				options.Cookie.SameSite = SameSiteMode.None;
				options.Cookie.MaxAge = TimeSpan.FromHours(8);
				options.ExpireTimeSpan = TimeSpan.FromHours(8);
				options.SlidingExpiration = true;
				var cookieDomain = configuration.GetValue<string>("CookieDomain");
				if (cookieDomain is null || cookieDomain == "")
					options.Cookie.Domain = cookieDomain;
			})
			.AddGoogleOpenIdConnect(options =>
		{
			options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.ClientId = configuration["Authentication:Google:ClientId"];
			options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
			options.SaveTokens = true;
			options.CallbackPath = new PathString("/auth/signin-google");
			options.Events.OnRedirectToIdentityProvider = context =>
			{
				var countryString = context.Request.Query["country"].ToString();
				var parsedCountry = Enum.Parse<Country>(countryString, true);
				var errorUrl = context.Request.Query["errorUrl"].ToString();
				var hd = parsedCountry switch
				{
					Country.Denmark => HttpUtility.UrlEncode("it-minds.dk"),
					Country.Norway => HttpUtility.UrlEncode("it-minds.no"),
					_ => throw new ArgumentOutOfRangeException()
				};
				context.ProtocolMessage.SetParameter("hd", hd);
				context.Properties.SetString("errorUrl", errorUrl);
				context.Properties.SetString("expectedHd", hd);
				return Task.CompletedTask;
			};
			options.Events.OnTokenResponseReceived = context =>
			{
				var expectedHd = context.Properties.GetString("expectedHd");
				var hdClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == "hd");
				if (hdClaim == null || hdClaim.Value != expectedHd)
				{
					var errorUrl = context.Properties.GetString("errorUrl");
					context.Response.Redirect($"{errorUrl}?error={HttpUtility.UrlDecode("login domain used is either not set or invalid")}");
					context.HandleResponse();
				}
				return Task.CompletedTask;
			};
			options.GetClaimsFromUserInfoEndpoint = true;
			options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
			options.RequireHttpsMetadata = true;
			options.NonceCookie.Name = "Pelican.Google.Nonce";
			options.CorrelationCookie.Name = "Pelican.Google.Correlation";
		});
		services.AddAuthorization(options =>
		{
			options.AddPolicy(AccessPoliciesStrings.Own, policy => policy.AddRequirements(new OwnRequirement()));
		});
		//services.AddSingleton<IClaimsTransformation, GoogleClaimsTransformation>();
		//services.AddSingleton<IEmployeeClaimsService, EmployeeClaimsService>();
		services.AddSingleton<IAuthorizationHandler, IsDirectorAuthorizationHandler>();
		services.AddHttpContextAccessor();
		services.AddLazyCache();
		services.AddControllers();

		return services;
	}
}
