using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotOwnerService : HubSpotService, IHubSpotOwnerService
{
	public HubSpotOwnerService(
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotSettings)
	{
	}

	public async Task<Result> GetOwnersAsync(string accessToken, CancellationToken cancellationToken)
	{

		RestRequest request = new RestRequest("crm/v3/owners")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnersResponse> response = await _client.ExecutePostAsync<OwnersResponse>(request, cancellationToken);


		return response.IsSuccessful && response.Data is not null
			? Result.Success()
			: Result.Failure<Tuple<string, string>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));

		throw new NotImplementedException();
	}
}
