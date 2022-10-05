using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses;

internal sealed class AccessTokenResponse
{
	[JsonPropertyName("user_id")]
	public long UserId { get; set; }
}
