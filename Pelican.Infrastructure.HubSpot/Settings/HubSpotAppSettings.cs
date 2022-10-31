namespace Pelican.Infrastructure.HubSpot.Settings;

public sealed class HubSpotAppSettings
{
	public int AppId { get; set; }
	public string ClientId { get; set; } = string.Empty;
	public string ClientSecret { get; set; } = string.Empty;
}
