using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Services;

[assembly: InternalsVisibleTo("Pelican.Infrastructure.HubSpot.Test")]
namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(
		this IServiceCollection services)
	{
		services.AddSingleton<IHubSpotClient, RestSharpHubSpotClient>();
		services.AddSingleton<IHubSpotOwnersService, HubSpotAccountManagerService>();
		services.AddSingleton<IHubSpotAuthorizationService, HubSpotAuthorizationService>();
		services.AddSingleton<IHubSpotObjectService<Contact>, HubSpotContactService>();
		services.AddSingleton<IHubSpotObjectService<Client>, HubSpotClientService>();
		services.AddSingleton<IHubSpotObjectService<Deal>, HubSpotDealService>();

		return services;
	}
}
