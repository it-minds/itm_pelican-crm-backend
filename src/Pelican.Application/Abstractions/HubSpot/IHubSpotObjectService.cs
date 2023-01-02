using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotObjectService<TEntity>
{
	Task<Result<TEntity>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken);

	Task<Result<List<TEntity>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken);
}
