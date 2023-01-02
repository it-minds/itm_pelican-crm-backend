using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

namespace Pelican.Infrastructure.HubSpot.Mapping.Auth;

internal static class RefreshAccessTokenResponseToAccessToken
{
	internal static string ToAccessToken(this RefreshAccessTokenResponse response)
		=> response.AccessToken;
}
