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

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			Client result = response.Data.ToClient();

			return Result.Success(result);
		}

		return Result.Failure<Client>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
	}

	public async Task<Result<List<Client>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/companies")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		RestResponse<CompaniesResponse> response = await _client
			.ExecuteGetAsync<CompaniesResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			List<Client> results = new();

			response
				.Data
				.Results
				.ToList()
				.ForEach(contactResponse =>
				{
					var result = contactResponse.ToClient();
					results.Add(result);
				});

			return Result.Success(results);
		}

		return Result.Failure<List<Client>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}
}
