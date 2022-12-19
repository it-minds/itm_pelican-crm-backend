using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;
public interface IHubSpotAuthorizationService
{
	Task<Result<RefreshAccessTokens>> AuthorizeUserAsync(string code, CancellationToken cancellationToken);

	Task<Result<Supplier>> DecodeAccessTokenAsync(string accessToken, CancellationToken cancellationToken);

	Task<Result<string>> RefreshAccessTokenFromSupplierHubSpotIdAsync(long supplierHubSpotId, CancellationToken cancellationToken);

	Task<Result<string>> RefreshAccessTokenFromRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
