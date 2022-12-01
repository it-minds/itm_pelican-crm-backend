namespace Pelican.Domain.Settings.HubSpot;

public sealed class HubSpotSettings : BaseSettings
{
	public string RedirectUrl { get; set; } = string.Empty;
	public HubSpotAppSettings App { get; set; } = new();
}
