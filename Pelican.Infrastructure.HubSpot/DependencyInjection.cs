using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Abstractions;
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

		services.AddSingleton<IHubSpotClient, RestSharpHubSpotClient>();

		services.AddSingleton<IHubSpotObjectService<AccountManager>, HubSpotAccountManagerService>();
		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotObjectService<Contact>, HubSpotContactService>();
		services.AddSingleton<IHubSpotObjectService<Client>, HubSpotClientService>();
		services.AddSingleton<IHubSpotObjectService<Deal>, HubSpotDealService>();

		return services;
	}
}
