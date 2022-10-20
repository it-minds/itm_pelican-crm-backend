using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

namespace Pelican.Infrastructure.HubSpot.Mapping.Auth;

internal static class RefreshAccessTokenResponseToString
{
	internal static string ToString(this RefreshAccessTokenResponse response)
		=> response.AccessToken;
}
