using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotDealService : HubSpotService, IHubSpotObjectService<Deal>
{
	public HubSpotDealService(
		IOptions<HubSpotSettings> hubSpotSettings) : base(hubSpotSettings)
	{ }

	public async Task<Result<Deal>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		RestResponse<DealResponse> response = await _client.ExecuteGetAsync<DealResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			Deal result = response.Data.ToDeal();

			return Result.Success(result);
		}

		return Result.Failure<Deal>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
	}

	public async Task<Result<List<Deal>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/deals")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		RestResponse<DealsResponse> response = await _client.ExecuteGetAsync<DealsResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			List<Deal> results = new();

			response
				.Data
				.Results
				.ToList()
				.ForEach(res =>
				{
					var r = res.ToDeal();

					ress.Add(r);
				});

			return Result.Success(ress);
		}

		return Result.Failure<List<Deal>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}
}
