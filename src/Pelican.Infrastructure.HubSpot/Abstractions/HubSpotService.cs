namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal abstract class HubSpotService
{
	protected readonly IHubSpotClient _hubSpotClient;

	protected HubSpotService(
		IHubSpotClient hubSpotClient)
	{
		_hubSpotClient = hubSpotClient;
	}
}
