namespace Pelican.Infrastructure.HubSpot;

public sealed class HubSpotSettings
{
	public string? BaseUrl { get; set; }
	public string? RedirectUrl { get; set; }
	public HubSpotAppSettings? App { get; set; }
}

public sealed class HubSpotAppSettings
{
	public int AppId { get; set; }
	public string? ClientId { get; set; }
	public string? ClientSecret { get; set; }
}
