namespace Pelican.Infrastructure.HubSpot.Settings;

public sealed class HubSpotAppSettings
{
	public int AppId { get; set; }
	public string ClientId { get; set; } = default!;
	public string ClientSecret { get; set; } = default!;
}
