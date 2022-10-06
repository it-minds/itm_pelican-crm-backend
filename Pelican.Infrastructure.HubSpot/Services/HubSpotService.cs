using Microsoft.Extensions.Options;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal abstract class HubSpotService
{
	protected readonly HubSpotSettings _hubSpotSettings;
	protected readonly RestClient _client;

	protected HubSpotService(
		IOptions<HubSpotSettings> hubSpotSettings)
	{
		if (hubSpotSettings is null
			|| hubSpotSettings.Value is null
			|| hubSpotSettings.Value.BaseUrl is null)
		{
			throw new ArgumentNullException(nameof(hubSpotSettings));
		}

		_hubSpotSettings = hubSpotSettings.Value;
		_client = new RestClient(hubSpotSettings.Value.BaseUrl);
	}
}
