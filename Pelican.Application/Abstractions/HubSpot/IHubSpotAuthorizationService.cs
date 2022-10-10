using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;
public interface IHubSpotAuthorizationService
{
	Task<Result<Tuple<string,string>>> AuthorizeUserAsync(string code, CancellationToken cancellationToken);

	Task<Result<long>> DecodeAccessTokenAsync(string accessToken, CancellationToken cancellationToken);

	Task<Result<string>> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
