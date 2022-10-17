using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotAccountManagerService
{
	Task<Result<AccountManager>> GetAccountManagerByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken);

	Task<Result<IEnumerable<AccountManager>>> GetAccountManagersAsync(
		string accessToken,
		CancellationToken cancellationToken);
}
