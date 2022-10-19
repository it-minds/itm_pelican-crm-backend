using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

[assembly: InternalsVisibleTo("Pelican.Infrastructure.HubSpot.Test")]
namespace Pelican.Infrastructure.HubSpot.Services;


internal sealed class HubSpotAuthorizationService : HubSpotService, IHubSpotAuthorizationService
{
	public HubSpotAuthorizationService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{ }

	public async Task<Result<RefreshAccessTokens>> AuthorizeUserAsync(
		string code,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("code", code, false)
			.AddQueryParameter("grant_type", "authorization_code", false);

		RestResponse<GetAccessTokenResponse> response = await _client
			.ExecutePostAsync<GetAccessTokenResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			RefreshAccessTokens result = response.Data.ToRefreshAccessTokens();

			return Result.Success(result);
		}

		return Result.Failure<RefreshAccessTokens>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
	}

	public async Task<Result<Supplier>> DecodeAccessTokenAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"oauth/v1/access-tokens/{accessToken}");

		RestResponse<AccessTokenResponse> response =
			await _client.ExecuteGetAsync<AccessTokenResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			Supplier result = response.Data.ToSupplier();

			return Result.Success(result);
		}

		return Result.Failure<Supplier>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
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

		RestResponse<RefreshAccessTokenResponse> response = await _client
			.ExecutePostAsync<RefreshAccessTokenResponse>(
				request,
				cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			string result = response.Data.AccessToken;

			return Result.Success(result);
		}

		return Result.Failure<string>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
	}
}
