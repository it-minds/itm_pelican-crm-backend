using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings;
using Pelican.Infrastructure.HubSpot.Services;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Pelican.Infrastructure.HubSpot.Test")]
namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(
		this IServiceCollection services)
	{
		services.AddSingleton<IHubSpotOwnersService, HubSpotAccountManagerService>();
		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotObjectService<Contact>, HubSpotContactService>();
		services.AddSingleton<IHubSpotObjectService<Client>, HubSpotClientService>();
		services.AddSingleton<IHubSpotObjectService<Deal>, HubSpotDealService>();

		return services;
	}
}
