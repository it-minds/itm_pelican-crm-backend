namespace Pelican.Infrastructure.HubSpot.Settings;

public sealed class HubSpotSettings
{
	public string BaseUrl { get; set; } = default!;
	public string RedirectUrl { get; set; } = default!;
	public HubSpotAppSettings App { get; set; } = default!;
}
