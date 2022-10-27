using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAuthorizationService : HubSpotService, IHubSpotAuthorizationService
{
	private readonly HubSpotSettings _hubSpotSettings;
	public HubSpotAuthorizationService(
		IHubSpotClient hubSpotClient,
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotClient)
	{
		_hubSpotSettings = hubSpotSettings.Value;
	}

	public async Task<Result<RefreshAccessTokens>> AuthorizeUserAsync(
		string code,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("code", code, false)
			.AddQueryParameter("grant_type", "authorization_code", false);

		RestResponse<GetAccessTokenResponse> response = await _hubSpotClient
			.PostAsync<GetAccessTokenResponse>(request, cancellationToken);

		return response
			.GetResult(GetAccessTokenResponseToRefreshAccessTokens.ToRefreshAccessTokens);
	}

	public async Task<Result<Supplier>> DecodeAccessTokenAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"oauth/v1/access-tokens/{accessToken}");

		RestResponse<AccessTokenResponse> response = await _hubSpotClient
			.GetAsync<AccessTokenResponse>(request, cancellationToken);

		return response
			.GetResult(AccessTokenResponseToSupplier.ToSupplier);
	}

	public async Task<Result<string>> RefreshAccessTokenAsync(
		string refreshToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("grant_type", "refresh_token", false)
			.AddQueryParameter("refresh_token", refreshToken, false);

		RestResponse<RefreshAccessTokenResponse> response = await _hubSpotClient
			.PostAsync<RefreshAccessTokenResponse>(
				request,
				cancellationToken);

		return response
			.GetResult(RefreshAccessTokenResponseToAccessToken.ToAccessToken);
	}
}
