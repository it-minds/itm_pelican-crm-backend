using System.Text.Json.Serialization;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.Abstractions.Pipedrive;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.Pipedrive;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;
using Pelican.Infrastructure.Pipedrive.Mapping.Deals;
using RestSharp;

namespace Pelican.Infrastructure.Pipedrive.Services;

public class PipedriveDealService : ServiceBase<PipedriveSettings>, IPipedriveService<Deal>
{

	public PipedriveDealService(IClient<PipedriveSettings> client)
		: base(client)
	{ }

	public async Task<Result<Deal>> GetByIdAsync(
		string clientDomain,
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"https://{clientDomain}.pipedrive.com/api/v1/deals/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		IResponse<DealResponse> response = await _client
			.GetAsync<DealResponse>(request, cancellationToken);

		return response
			.GetResult(DealResponseToDeal.ToDeal);
	}

	public async Task<Result<List<Deal>>> GetAsync(
		string clientDomain,
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"https://{clientDomain}.pipedrive.com/api/v1/deals")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		IResponse<DealsResponse> response = await _client
			.GetAsync<DealsResponse>(request, cancellationToken);

		return response
			.GetResult(DealsResponseToDeals.ToDeals);
	}
}

