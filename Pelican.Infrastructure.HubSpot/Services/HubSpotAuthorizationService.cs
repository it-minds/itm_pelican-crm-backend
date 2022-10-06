﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses;
using RestSharp;

[assembly: InternalsVisibleTo("Pelican.Infrastructure.HubSpot.Test")]
namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAuthorizationService : HubSpotService, IHubSpotAuthorizationService
{
	public HubSpotAuthorizationService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{
	}

	public async Task<Result> AuthorizeUserAsync(
		string code,
		CancellationToken cancellationToken)
	{
		if (_hubSpotSettings is null
			|| _hubSpotSettings.App is null
			|| _hubSpotSettings.App.ClientId is null
			|| _hubSpotSettings.RedirectUrl is null
			|| _hubSpotSettings.App.ClientSecret is null)
		{
			return Result.Failure(Error.NullValue);
		}

		RestRequest request = new RestRequest("oauth/v1/token")
			.AddHeader("Content-Type", "application/x-www-form-urlencoded")
			.AddHeader("charset", "utf-8")
			.AddQueryParameter("code", code, false)
			.AddQueryParameter("client_id", _hubSpotSettings.App.ClientId, false)
			.AddQueryParameter("redirect_uri", _hubSpotSettings.RedirectUrl, false)
			.AddQueryParameter("client_secret", _hubSpotSettings.App.ClientSecret, false)
			.AddQueryParameter("grant_type", "authorization_code", false);

		RestResponse response = await _client.ExecutePostAsync(request, cancellationToken);

		return response.IsSuccessful
			? Result.Success()
			: Result.Failure(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}

	public async Task<Result<long>> DecodeAccessTokenAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"oauth/v1/access-tokens/{accessToken}");

		RestResponse<AccessTokenResponse> response =
			await _client.ExecuteGetAsync<AccessTokenResponse>(request, cancellationToken);

		return response.IsSuccessful && response.Data is not null
			? Result.Success(response.Data.UserId)
			: Result.Failure<long>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}

	public async Task<Result<string>> RefreshAccessTokenAsync(
		string refreshToken,
		CancellationToken cancellationToken)
	{
		if (_hubSpotSettings is null
			|| _hubSpotSettings.App is null
			|| _hubSpotSettings.App.ClientId is null
			|| _hubSpotSettings.RedirectUrl is null
			|| _hubSpotSettings.App.ClientSecret is null)
		{
			return Result.Failure<string>(Error.NullValue);
		}

		RestRequest request = new RestRequest("oauth/v1/token")
			.AddHeader("Content-Type", "application/x-www-form-urlencoded")
			.AddHeader("charset", "utf-8")
			.AddQueryParameter("client_id", _hubSpotSettings.App.ClientId, false)
			.AddQueryParameter("redirect_uri", _hubSpotSettings.RedirectUrl, false)
			.AddQueryParameter("client_secret", _hubSpotSettings.App.ClientSecret, false)
			.AddQueryParameter("grant_type", "refresh_token", false)
			.AddQueryParameter("refresh_token", refreshToken);

		RestResponse<RefreshAccessTokenResponse> response =
			await _client.ExecutePostAsync<RefreshAccessTokenResponse>(
				request,
				cancellationToken);

		return response.IsSuccessful
			&& response.Data?.AccessToken is not null
			? Result.Success(response.Data.AccessToken)
			: Result.Failure<string>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}
}
