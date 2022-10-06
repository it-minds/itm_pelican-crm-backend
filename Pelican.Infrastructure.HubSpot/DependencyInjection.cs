using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Infrastructure.HubSpot.Services;

namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<HubSpotSettings>(configuration.GetSection(nameof(HubSpotSettings)));

		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotDealService, HubSpotDealService>();

		return services;
	}
}
