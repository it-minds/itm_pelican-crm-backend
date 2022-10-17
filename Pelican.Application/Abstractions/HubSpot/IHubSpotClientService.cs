using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotClientService
{
	Task<Result<Client>> GetClientByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken);

	Task<Result<IEnumerable<Client>>> GetClientsAsync(
		string accessToken,
		CancellationToken cancellationToken);
}
