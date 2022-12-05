namespace Pelican.Domain.Settings.HubSpot;

public sealed class HubSpotSettings
{
	public string BaseUrl { get; set; } = string.Empty;
	public string RedirectUrl { get; set; } = string.Empty;
	public HubSpotAppSettings App { get; set; } = new();
}
