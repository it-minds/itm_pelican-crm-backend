using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

public class DealByIdDataLoader : BatchDataLoader<Guid, Deal>, IDealByIdDataLoader
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public DealByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<PelicanContext> dbContextFactory) : base(batchScheduler)
	{
		_dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}
	protected override async Task<IReadOnlyDictionary<Guid, Deal>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		await using var pelicanContext = _dbContextFactory.CreateDbContext();
		return await pelicanContext.Deals.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
