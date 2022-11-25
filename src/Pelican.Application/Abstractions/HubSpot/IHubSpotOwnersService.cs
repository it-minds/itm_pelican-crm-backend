using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotOwnersService
{
	Task<Result<AccountManager>> GetByUserIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken);

	Task<Result<List<AccountManager>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken);
}
