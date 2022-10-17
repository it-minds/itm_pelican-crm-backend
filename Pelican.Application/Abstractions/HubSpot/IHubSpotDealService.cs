using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;
public interface IHubSpotDealService
{
	Task<Result<Deal>> GetDealByIdAsync(string refreshToken, long id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Deal>>> GetDealsAsync(string accessToken, CancellationToken cancellationToken);
}
