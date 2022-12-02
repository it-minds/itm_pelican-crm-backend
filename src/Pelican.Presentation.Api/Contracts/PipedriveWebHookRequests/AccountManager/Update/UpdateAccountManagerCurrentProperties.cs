using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.Update;
public record UpdateAccountManagerCurrentProperties
{
	[JsonPropertyName("name")]
	public string AccountManagerFullName { get; set; } = string.Empty;

	[JsonPropertyName("icon_url")]
	public string? PictureUrl { get; set; } = string.Empty;

	[JsonPropertyName("phone")]
	public string? PhoneNumber { get; set; } = string.Empty;

	[JsonPropertyName("email")]
	public string? Email { get; set; } = string.Empty;
}
