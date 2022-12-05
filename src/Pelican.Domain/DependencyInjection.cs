using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Settings.Pipedrive;

namespace Pelican.Domain;

public static class DependencyInjection
{
	public static IServiceCollection AddDomain(
		this IServiceCollection services,
		IConfiguration configuration, bool isProduction)
	{
		IConfigurationSection hubSpotSettings;
		IConfigurationSection pipedriveSettings;
		if (isProduction)
		{
			string keyVaultName = configuration["KeyVaultName"];
			var kvUri = "https://" + keyVaultName + ".vault.azure.net";
			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

			hubSpotSettings = configuration.GetRequiredSection("HubSpotDemoSettings");
			hubSpotSettings["App:ClientId"] = client.GetSecret("HubSpotAppClientID").Value.Value;
			hubSpotSettings["App:ClientSecret"] = client.GetSecret("HubSpotAppClientSecret").Value.Value;

			pipedriveSettings = configuration.GetRequiredSection("PipedriveDemoSettings");
			pipedriveSettings["App:ClientId"] = client.GetSecret("PipedriveAppClientId").Value.Value;
			pipedriveSettings["App:ClientSecret"] = client.GetSecret("PipedriveAppClientSecret").Value.Value;
		}
		else
		{
			hubSpotSettings = configuration.GetSection("HubSpotDevSettings");
			pipedriveSettings = configuration.GetSection("PipedriveDevSettings");
		}
		if (hubSpotSettings is null)
		{
			throw new NullReferenceException(nameof(hubSpotSettings));
		}
		if (pipedriveSettings is null)
		{
			throw new NullReferenceException(nameof(pipedriveSettings));
		}
		services.Configure<HubSpotSettings>(hubSpotSettings);
		services.Configure<PipedriveSettings>(pipedriveSettings);

		return services;
	}
}
