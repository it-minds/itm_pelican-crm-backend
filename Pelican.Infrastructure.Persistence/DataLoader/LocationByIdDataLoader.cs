using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Infrastructure.Persistence;
using Location = Pelican.Domain.Entities.Location;

public class LocationByIdDataLoader : BatchDataLoader<Guid, Location>, ILocationByIdDataLoader
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public LocationByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<PelicanContext> dbContextFactory) : base(batchScheduler)
	{
		_dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}
	protected override async Task<IReadOnlyDictionary<Guid, Location>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		await using var pelicanContext = _dbContextFactory.CreateDbContext();
		return await pelicanContext.Locations.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
