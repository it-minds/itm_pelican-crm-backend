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
		IConfigurationSection hubSpotSettings;
		if (isProduction)
		{
			string keyVaultName = configuration["KeyVaultName"];
			var kvUri = "https://" + keyVaultName + ".vault.azure.net";
			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
			hubSpotSettings = configuration.GetRequiredSection("HubSpotDemoSettings");
			hubSpotSettings["App:HubSpotClientId"] = client.GetSecret("HubSpotAppClientID").Value.Value;
			hubSpotSettings["App:HubSpotClientSecret"] = client.GetSecret("HubSpotAppClientSecret").Value.Value;
		}
		else
		{
			hubSpotSettings = configuration.GetSection("HubSpotDevSettings");
		}
		if (hubSpotSettings is null)
		{
			throw new NullReferenceException(nameof(hubSpotSettings));
		}
		services.Configure<HubSpotSettings>(hubSpotSettings);
		return services;
	}
}
