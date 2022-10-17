﻿using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAccountManagerService : HubSpotService, IHubSpotAccountManagerService
{
	public HubSpotAccountManagerService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{ }

	public async Task<Result<AccountManager>> GetAccountManagerByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v3/owners/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnerResponse> response = await _client
			.ExecuteGetAsync<OwnerResponse>(request, cancellationToken);

		return response.IsSuccessful
			&& response.Data is not null
			? Result.Success(
				response
				.Data.ToAccountManager())
			: Result.Failure<AccountManager>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}

	public async Task<Result<IEnumerable<AccountManager>>> GetAccountManagersAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v3/owners")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnersResponse> response = await _client
			.ExecuteGetAsync<OwnersResponse>(request, cancellationToken);

		return response.IsSuccessful
			&& response.Data is not null
			? Result.Success(
				response
				.Data
				.Results
				.Select(owner => owner.ToAccountManager())
				.AsEnumerable())
			: Result.Failure<IEnumerable<AccountManager>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException!.Message));
	}
}