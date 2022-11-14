namespace Pelican.Domain.Settings;

public sealed class HubSpotAppSettings
{
	public int AppId { get; set; }
	public string HubSpotClientId { get; set; } = string.Empty;
	public string HubSpotClientSecret { get; set; } = string.Empty;
}
