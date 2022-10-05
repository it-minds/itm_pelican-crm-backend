using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal class HubSpotDealService : HubSpotService, IHubSpotDealService
{
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;

	public HubSpotDealService(
		IOptions<HubSpotSettings> hubSpotSettings,
		IHubSpotAuthorizationService hubSpotAuthorizationService) : base(hubSpotSettings)
	{
		_hubSpotAuthorizationService = hubSpotAuthorizationService;
	}

	public async Task<Result<Deal>> GetDealByIdAsync(
		string refreshToken,
		long id,
		CancellationToken cancellationToken)
	{
		Result<string> accessTokenResult =
			await _hubSpotAuthorizationService.RefreshAccessTokenAsync(refreshToken, cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Deal>(accessTokenResult.Error);
		}

		RestRequest request = new RestRequest($"crm/v3/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessTokenResult.Value}");

		RestResponse<Deal> response = await _client.ExecuteGetAsync<Deal>(request, cancellationToken);

		return (response.IsSuccessful
			? Result.Success(response.Data!)
			: Result.Failure<Deal>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!)));
	}
}
