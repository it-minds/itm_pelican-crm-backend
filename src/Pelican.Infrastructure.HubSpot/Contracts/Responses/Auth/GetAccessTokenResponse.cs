using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

internal sealed class GetAccessTokenResponse
{
	[JsonPropertyName("token_type")]
	public string TokenType { get; set; } = string.Empty;

	[JsonPropertyName("refresh_token")]
	public string RefreshToken { get; set; } = string.Empty;

	[JsonPropertyName("access_token")]
	public string AccessToken { get; set; } = string.Empty;

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; set; }

}
