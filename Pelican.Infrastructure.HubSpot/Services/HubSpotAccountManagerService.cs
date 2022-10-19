using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAccountManagerService : HubSpotService, IHubSpotObjectService<AccountManager>
{
	public HubSpotAccountManagerService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{ }

	public async Task<Result<AccountManager>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v3/owners/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnerResponse> response = await _client
			.ExecuteGetAsync<OwnerResponse>(request, cancellationToken);

		if (response.IsSuccessful && response.Data is not null)
		{
			AccountManager result = response
				.Data
				.ToAccountManager();

			return Result.Success(result);
		}

		return Result.Failure<AccountManager>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}

	public async Task<Result<List<AccountManager>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v3/owners")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnersResponse> response = await _client
			.ExecuteGetAsync<OwnersResponse>(request, cancellationToken);

		if (response.IsSuccessful && response.Data is not null)
		{
			List<AccountManager> results = new();

			response
				.Data
				.Results
				.ToList()
				.ForEach(contactResponse =>
				{
					var result = contactResponse.ToAccountManager();
					results.Add(result);
				});

			return Result.Success(results);
		}

		return Result.Failure<List<AccountManager>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}
}
