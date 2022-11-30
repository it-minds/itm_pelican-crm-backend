using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Deals;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotDealService : ServiceBase, IHubSpotObjectService<Deal>
{
	public HubSpotDealService(
		IClient hubSpotClient) : base(hubSpotClient)
	{ }

	public async Task<Result<Deal>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/deals/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		RestResponse<DealResponse> response = await _client
			.GetAsync<DealResponse>(request, cancellationToken);

		return response
			.GetResult(DealResponseToDeal.ToDeal);
	}

	public async Task<Result<List<Deal>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/deals")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		RestResponse<DealsResponse> response = await _client
			.GetAsync<DealsResponse>(request, cancellationToken);

		return response
			.GetResult(DealsResponseToDeals.ToDeals);
	}
}
