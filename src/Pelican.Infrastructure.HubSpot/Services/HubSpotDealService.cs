using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Deals;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotDealService : ServiceBase<HubSpotSettings>, IHubSpotObjectService<Deal>
{
	private readonly IUnitOfWork _unitOfWork;

	public HubSpotDealService(
		IClient<HubSpotSettings> hubSpotClient,
		IUnitOfWork unitOfWork)
		: base(hubSpotClient, unitOfWork)
	{ }

	public async Task<Result<Deal>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/deals/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddDealQueryParams();

		IResponse<DealResponse> response = await _client
			.GetAsync<DealResponse>(
				request,
				cancellationToken);

		return await response
			.GetResultWithUnitOfWork(
				DealResponseToDeal.ToDeal,
				_unitOfWork,
				cancellationToken);
	}

	public async Task<Result<List<Deal>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		var responses = await GetAllPages(accessToken, cancellationToken).ToListAsync(cancellationToken);

		var results = responses
			.Select(response => response
				.GetResultWithUnitOfWork(
					DealsResponseToDeals.ToDeals,
					_unitOfWork,
					cancellationToken))
			.Select(t => t.Result)
			.ToArray();

		return Result.FirstFailureOrSuccess(results) is Result result && result.IsFailure
			? (Result<List<Deal>>)result
			: results.SelectMany(r => r.Value).ToList();
	}

	private async IAsyncEnumerable<IResponse<PaginatedResponse<DealResponse>>> GetAllPages(
		string accessToken,
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		string after = "0";
		while (!string.IsNullOrWhiteSpace(after))
		{
			RestRequest request = new RestRequest("crm/v4/objects/deals")
				.AddHeader("Authorization", $"Bearer {accessToken}")
				.AddDealQueryParams()
				.AddQueryParameter("after", after, false);

			IResponse<PaginatedResponse<DealResponse>> responses = await _client
				.GetAsync<PaginatedResponse<DealResponse>>(
					request,
					cancellationToken);

			after = responses.After();

			yield return responses;
		}
	}
}
