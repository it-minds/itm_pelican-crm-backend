using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotClientService : HubSpotService, IHubSpotObjectService<Client>
{
	public HubSpotClientService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{ }

	public async Task<Result<Client>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/companies/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		RestResponse<CompanyResponse> response = await _client
			.ExecuteGetAsync<CompanyResponse>(request, cancellationToken);

		return response.IsSuccessful
			&& response.Data is not null
			? Result.Success(
				response
				.Data.ToClient())
			: Result.Failure<Client>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}

	public async Task<Result<IEnumerable<Client>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/companies")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		RestResponse<CompaniesResponse> response = await _client
			.ExecuteGetAsync<CompaniesResponse>(request, cancellationToken);

		return response.IsSuccessful
			&& response.Data is not null
			? Result.Success(
				response
				.Data
				.Results
				.Select(company => company.ToClient())
				.AsEnumerable())
			: Result.Failure<IEnumerable<Client>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}
}
