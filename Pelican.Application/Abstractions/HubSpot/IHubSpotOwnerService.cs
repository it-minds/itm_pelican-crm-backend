using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotOwnerService
{
	Task<Result> GetOwnersAsync(string accessToken, CancellationToken cancellationToken);
}
