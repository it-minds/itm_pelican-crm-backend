using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Primitives;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence;

public class PelicanContext : DbContext, IPelicanContext
{
	public PelicanContext(DbContextOptions<PelicanContext> options) : base(options)
	{
	}
	public PelicanContext()
	{

	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PelicanContext).Assembly);
	}


	public override int SaveChanges()
	{
		SetTimeTrackedValues();
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		SetTimeTrackedValues();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		SetTimeTrackedValues();
		return base.SaveChangesAsync(cancellationToken);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
	{
		SetTimeTrackedValues();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	private void SetCreatedAtOnAddedEntities()
	{
		ChangeTracker.Entries()
			.Where(t =>
				t.Entity is ITimeTracked &&
				t.State == EntityState.Added)
			.Select(t => t.Entity as ITimeTracked)
			.ToList()
			.ForEach(entity =>
			{
				if (entity is not null)
				{
					entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				}
			});
	}
	private void SetLastUpdatedAtOnUpdatedEntities()
	{
		ChangeTracker.Entries()
			.Where(t =>
				t.Entity is ITimeTracked &&
				t.State == EntityState.Modified)
			.Select(t => t.Entity as ITimeTracked)
			.ToList()
			.ForEach(entity =>
			{
				if (entity is not null)
				{
					entity.LastUpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				}
			});
	}
	private void SetTimeTrackedValues()
	{
		SetCreatedAtOnAddedEntities();
		SetLastUpdatedAtOnUpdatedEntities();
	}
}
