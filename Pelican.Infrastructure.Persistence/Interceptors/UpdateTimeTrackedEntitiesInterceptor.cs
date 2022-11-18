
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.Interceptors;

internal sealed class UpdateTimeTrackedEntitiesInterceptor
	: SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		DbContext? dbContext = eventData.Context;

		if (dbContext is null)
		{
			return base.SavingChangesAsync(
				eventData,
				result,
				cancellationToken);
		}

		SetCreatedAtOnAddedEntities(dbContext);
		SetLastUpdatedAtOnUpdatedEntities(dbContext);

		return base.SavingChangesAsync(
			eventData,
			result,
			cancellationToken);
	}

	private void SetCreatedAtOnAddedEntities(DbContext dbContext)
	{
		dbContext.ChangeTracker
			.Entries<ITimeTracked>()
			.Where(entityEntry => entityEntry.State == EntityState.Added)
			.Select(entityEntry => entityEntry.Entity)
			.ToList()
			.ForEach(entity =>
			{
				if (entity is not null)
				{
					entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				}
			});
	}

	private void SetLastUpdatedAtOnUpdatedEntities(DbContext dbContext)
	{
		dbContext.ChangeTracker
			.Entries<ITimeTracked>()
			.Where(entityEntry => entityEntry.State == EntityState.Modified)
			.Select(entityEntry => entityEntry.Entity)
			.ToList()
			.ForEach(entity =>
			{
				if (entity is not null)
				{
					entity.LastUpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				}
			});
	}
}
