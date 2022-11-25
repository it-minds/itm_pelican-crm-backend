using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

internal sealed class AccessTokenResponse
{
	[JsonPropertyName("user")]
	public string User { get; init; } = string.Empty;

	[JsonPropertyName("hub_domain")]
	public string HubDomain { get; init; } = string.Empty;

	[JsonPropertyName("hub_id")]
	public long HubId { get; init; }

	[JsonPropertyName("user_id")]
	public long UserId { get; init; }
}
