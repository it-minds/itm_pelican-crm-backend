using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Infrastructure.HubSpot.Services;
using Pelican.Infrastructure.HubSpot.Settings;

namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<HubSpotSettings>(configuration.GetSection(nameof(HubSpotSettings)));

		services.AddSingleton<IHubSpotAccountManagerService, HubSpotAccountManagerService>();
		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotContactService, HubSpotContactService>();
		services.AddSingleton<IHubSpotClientService, HubSpotClientService>();
		services.AddSingleton<IHubSpotDealService, HubSpotDealService>();

		return services;
	}
}
