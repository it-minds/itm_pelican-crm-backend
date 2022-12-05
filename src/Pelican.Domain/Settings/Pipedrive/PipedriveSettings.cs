namespace Pelican.Domain.Settings.Pipedrive;
public sealed class PipedriveSettings
{
	public string RedirectUrl { get; set; } = string.Empty;
	public PipedriveAppSettings App { get; set; } = new();
}
