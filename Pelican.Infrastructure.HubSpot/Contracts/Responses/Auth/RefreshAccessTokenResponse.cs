using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

internal sealed class RefreshAccessTokenResponse
{
	[JsonPropertyName("access_token")]
	public string AccessToken { get; set; } = default!;
}
