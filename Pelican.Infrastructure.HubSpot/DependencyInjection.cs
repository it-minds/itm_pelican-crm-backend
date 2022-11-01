using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Services;

[assembly: InternalsVisibleTo("Pelican.Infrastructure.HubSpot.Test")]
namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		IConfigurationSection hubSpotSettings = configuration.GetSection(nameof(HubSpotSettings));

		if (hubSpotSettings is null)
		{
			throw new ArgumentNullException(nameof(hubSpotSettings));
		}

		services.Configure<HubSpotSettings>(hubSpotSettings);

		services.AddSingleton<IHubSpotClient, RestSharpHubSpotClient>();

		services.AddSingleton<IHubSpotObjectService<AccountManager>, HubSpotAccountManagerService>();
		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotObjectService<Contact>, HubSpotContactService>();
		services.AddSingleton<IHubSpotObjectService<Client>, HubSpotClientService>();
		services.AddSingleton<IHubSpotObjectService<Deal>, HubSpotDealService>();

		return services;
	}
}
