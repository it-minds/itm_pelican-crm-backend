using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<HubSpotSettings>(configuration.GetSection(nameof(HubSpotSettings)));

		//services.AddHttpClient("HubSpot", (provider, client) =>
		//{
		//	var settings = provider.GetRequiredService<IOptions<HubSpotSettings>>();

		//	client.BaseAddress = new Uri(settings.Value.BaseUrl
		//		?? throw new ArgumentNullException(nameof(HubSpotSettings)));
		//});

		return services;
	}
}
