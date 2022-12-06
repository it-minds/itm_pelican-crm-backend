using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Pipedrive;

public interface IPipedriveService<TEntity>
{
	Task<Result<TEntity>> GetByIdAsync(
		string clientDomain,
		string accessToken,
		long id,
		CancellationToken cancellationToken);

	Task<Result<List<TEntity>>> GetAsync(
		string clientDomain,
		string accessToken,
		CancellationToken cancellationToken);
}
