using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotClientService : ServiceBase<HubSpotSettings>, IHubSpotObjectService<Client>
{
	public HubSpotClientService(
		IClient<HubSpotSettings> hubSpotClient,
		IUnitOfWork unitOfWork)
		: base(hubSpotClient, unitOfWork)
	{ }

	public async Task<Result<Client>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/companies/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddCompanyQueryParams();

		IResponse<CompanyResponse> response = await _client
			.GetAsync<CompanyResponse>(request, cancellationToken);

		return await response
			.GetResultWithUnitOfWork(
				CompanyResponseToClient.ToClient,
				_unitOfWork,
				cancellationToken);
	}

	public async Task<Result<List<Client>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		var responses = await GetAllPages(accessToken, cancellationToken).ToListAsync(cancellationToken);

		var results = responses
			.Select(response => response
				.GetResultWithUnitOfWork(
					CompaniesResponseToClients.ToClients,
					_unitOfWork,
					cancellationToken))
			.Select(t => t.Result)
			.ToArray();

		if ((Result.FirstFailureOrSuccess(results) is Result result) && result.IsFailure)
		{
			return (Result<List<Client>>)result;
		}

		return results.SelectMany(r => r.Value).ToList();
	}

	private async IAsyncEnumerable<IResponse<PaginatedResponse<CompanyResponse>>> GetAllPages(
		string accessToken,
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		string after = "0";
		while (!string.IsNullOrWhiteSpace(after))
		{
			RestRequest request = new RestRequest("crm/v4/objects/companies")
				.AddHeader("Authorization", $"Bearer {accessToken}")
				.AddCompanyQueryParams()
				.AddQueryParameter("after", after, false);

			IResponse<PaginatedResponse<CompanyResponse>> responses = await _client
				.GetAsync<PaginatedResponse<CompanyResponse>>(
					request,
					cancellationToken);

			after = responses.After();

			yield return responses;
		}
	}
}
