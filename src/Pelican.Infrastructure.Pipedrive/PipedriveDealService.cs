using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Infrastructure.Pipedrive.Services;

public class PipedriveDealService
{
	public async Task<Result<Deal>> GetByIdAsync(
		string clientDomain,
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"v1/deals/{id}")
			.AddUrlSegment("clientDomain", clientDomain)
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		RestResponse<DealResponse> response = await _hubSpotClient
			.GetAsync<DealResponse>(request, cancellationToken);

		return response
			.GetResult(DealResponseToDeal.ToDeal);
	}

	public async Task<Result<List<Deal>>> GetAsync(
		string clientDomain,
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"v1/deals")
			.AddUrlSegment("clientDomain", clientDomain)
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.add
			.AddDealQueryParams();

		RestResponse<DealsResponse> response = await _hubSpotClient
			.GetAsync<DealsResponse>(request, cancellationToken);

		return response
			.GetResult(DealsResponseToDeals.ToDeals);
	}

}
