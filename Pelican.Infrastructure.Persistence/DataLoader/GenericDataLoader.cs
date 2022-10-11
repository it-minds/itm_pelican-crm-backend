using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.DataLoader;
public class GenericDataLoader<T> : BatchDataLoader<Guid, T>, IGenericDataLoader<T> where T : Entity
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public GenericDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<PelicanContext> dbContextFactory) : base(batchScheduler)
	{
		_dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}
	//This loads a batch from the database where the id is equal to the id requested
	protected override async Task<IReadOnlyDictionary<Guid, T>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		await using var pelicanContext = _dbContextFactory.CreateDbContext();
		return await pelicanContext.Set<T>()
			.Where(s => keys.Contains(s.Id))
			.ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
