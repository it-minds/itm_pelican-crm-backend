using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAccountManagerService : ServiceBase<HubSpotSettings>, IHubSpotOwnersService
{
	public HubSpotAccountManagerService(
		IClient<HubSpotSettings> hubSpotClient,
		IUnitOfWork unitOfWork)
		: base(hubSpotClient, unitOfWork)
	{ }

	public async Task<Result<AccountManager>> GetByUserIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v3/owners/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddQueryParameter("idProperty", "userid");

		IResponse<OwnerResponse> response = await _client
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

		IResponse<PaginatedResponse<OwnerResponse>> response = await _client
			.GetAsync<PaginatedResponse<OwnerResponse>>(request, cancellationToken);

		return response
			.GetResult(OwnersResponseToAccountManagers.ToAccountManagers);
	}
}
