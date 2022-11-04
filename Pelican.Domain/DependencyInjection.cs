using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Domain.Settings;

namespace Pelican.Domain;

public static class DependencyInjection
{
	public static IServiceCollection AddDomain(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		IConfigurationSection hubSpotSettings = configuration.GetSection(nameof(HubSpotSettings));

		if (hubSpotSettings is null)
		{
			throw new NullReferenceException(nameof(hubSpotSettings));
		}

		services.Configure<HubSpotSettings>(hubSpotSettings);

		return services;
	}
}
