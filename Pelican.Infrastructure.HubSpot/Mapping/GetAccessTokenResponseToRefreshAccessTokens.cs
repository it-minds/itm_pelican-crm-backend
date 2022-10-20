using Pelican.Application.HubSpot.Dtos;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

namespace Pelican.Infrastructure.HubSpot.Mapping;

internal static class GetAccessTokenResponseToRefreshAccessTokens
{
	internal static RefreshAccessTokens ToRefreshAccessTokens(this GetAccessTokenResponse response)
		=> new()
		{
			AccessToken = response.AccessToken,
			RefreshToken = response.RefreshToken,
		};
}
