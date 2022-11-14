using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Domain.Settings;

namespace Pelican.Domain;

public static class DependencyInjection
{
	public static IServiceCollection AddDomain(
		this IServiceCollection services,
		IConfiguration configuration, bool isProduction)
	{
		HubSpotSettings hubSpotSettings;
		IConfiguration configurationSettings;
		if (isProduction)
		{
			string keyVaultName = configuration["KeyVaultName"];
			var kvUri = "https://" + keyVaultName + ".vault.azure.net";
			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
			hubSpotSettings = configuration.GetSection("HubSpotDemoSettings").Get<HubSpotSettings>();
			configurationSettings = configuration.GetSection("HubSpotDemoSettings");
			configurationSettings["HubSpotClientId"] = client.GetSecret(hubSpotSettings.App.ClientId).Value.Value;
			configurationSettings["HubSpotClientSecret"] = client.GetSecret(hubSpotSettings.App.ClientSecret).Value.Value;
		}
		else
		{
			configurationSettings = configuration.GetSection("HubSpotDevSettings");
		}
		if (configurationSettings is null)
		{
			throw new NullReferenceException(nameof(configurationSettings));
		}
		services.Configure<HubSpotSettings>(configurationSettings);
		return services;
	}
}
