using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotClientService : HubSpotService, IHubSpotObjectService<Client>
{
	public HubSpotClientService(
		IHubSpotClient hubSpotClient)
		: base(hubSpotClient)
	{ }

	public async Task<Result<Client>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/companies/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		RestResponse<CompanyResponse> response = await _hubSpotClient
			.GetAsync<CompanyResponse>(request, cancellationToken);

		return response
			.GetResult(CompanyResponseToClient.ToClient);
	}

	public async Task<Result<List<Client>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/companies")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		RestResponse<CompaniesResponse> response = await _hubSpotClient
			.GetAsync<CompaniesResponse>(request, cancellationToken);

		return response
			.GetResult(CompaniesResponseToClients.ToClients);
	}
}
