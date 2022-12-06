using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pelican.Application.Abstractions.Data;

namespace Pelican.Infrastructure.Persistence;

public class PelicanContext : DbContext, IPelicanContext
{
	public PelicanContext(DbContextOptions<PelicanContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PelicanContext).Assembly);
	}

	public override DbSet<TEntity> Set<TEntity>()
		=> base.Set<TEntity>();

	public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
		=> base.Entry(entity);

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> base.SaveChangesAsync(cancellationToken);
}

