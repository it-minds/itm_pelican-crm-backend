using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotAuthorizationService : ServiceBase<HubSpotSettings>, IHubSpotAuthorizationService
{
	private readonly HubSpotSettings _hubSpotSettings;
	public HubSpotAuthorizationService(
		IClient<HubSpotSettings> hubSpotClient,
		IOptions<HubSpotSettings> hubSpotSettings)
		: base(hubSpotClient)
	{
		_hubSpotSettings = hubSpotSettings.Value ?? throw new ArgumentNullException(nameof(hubSpotSettings));
	}

	public async Task<Result<RefreshAccessTokens>> AuthorizeUserAsync(
		string code,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("code", code, false)
			.AddQueryParameter("grant_type", "authorization_code", false);

		IResponse<GetAccessTokenResponse> response = await _client
			.PostAsync<GetAccessTokenResponse>(request, cancellationToken);

		return response
			.GetResultV1(GetAccessTokenResponseToRefreshAccessTokens.ToRefreshAccessTokens);
	}

	public async Task<Result<Supplier>> DecodeAccessTokenAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"oauth/v1/access-tokens/{accessToken}");

		IResponse<AccessTokenResponse> response = await _client
			.GetAsync<AccessTokenResponse>(request, cancellationToken);

		return response
			.GetResultV1(AccessTokenResponseToSupplier.ToSupplier);
	}

	public async Task<Result<string>> RefreshAccessTokenFromSupplierHubSpotIdAsync(
		long supplierHubSpotId,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		Supplier? supplier = await unitOfWork
			.SupplierRepository
			.FindByCondition(supplier => supplier.SourceId == supplierHubSpotId && supplier.Source == Sources.HubSpot)
			.AsNoTracking()
			.FirstOrDefaultAsync();

		if (supplier is null || string.IsNullOrWhiteSpace(supplier.RefreshToken))
		{
			return Result.Failure<string>(Error.NullValue);
		}

		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("grant_type", "refresh_token", false)
			.AddQueryParameter("refresh_token", supplier.RefreshToken, false);

		IResponse<RefreshAccessTokenResponse> response = await _client
			.PostAsync<RefreshAccessTokenResponse>(
				request,
				cancellationToken);

		return response
			.GetResultV1(RefreshAccessTokenResponseToAccessToken.ToAccessToken);
	}

	public async Task<Result<string>> RefreshAccessTokenFromRefreshTokenAsync(
		string refreshToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("oauth/v1/token")
			.AddAuthorizationHeaders()
			.AddCommonAuthorizationQueryParams(_hubSpotSettings)
			.AddQueryParameter("grant_type", "refresh_token", false)
			.AddQueryParameter("refresh_token", refreshToken, false);

		IResponse<RefreshAccessTokenResponse> response = await _client
			.PostAsync<RefreshAccessTokenResponse>(
				request,
				cancellationToken);

		return response
			.GetResultV1(RefreshAccessTokenResponseToAccessToken.ToAccessToken);
	}
}
