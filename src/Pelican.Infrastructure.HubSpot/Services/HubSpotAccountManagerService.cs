using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAccountManagerService : HubSpotService, IHubSpotOwnersService
{
	public HubSpotAccountManagerService(
		IHubSpotClient hubSpotClient)
		: base(hubSpotClient)
	{ }

	public async Task<Result<AccountManager>> GetByUserIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v3/owners/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddQueryParameter("idProperty", "userid");

		RestResponse<OwnerResponse> response = await _hubSpotClient
			.GetAsync<OwnerResponse>(request, cancellationToken);

		return response
			.GetResult(OwnerResponseToAccountManager.ToAccountManager);
	}

	public async Task<Result<List<AccountManager>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v3/owners")
			.AddHeader("Authorization", $"Bearer {accessToken}");

		RestResponse<OwnersResponse> response = await _hubSpotClient
			.GetAsync<OwnersResponse>(request, cancellationToken);

		return response
			.GetResult(OwnersResponseToAccountManagers.ToAccountManagers);
	}
}
