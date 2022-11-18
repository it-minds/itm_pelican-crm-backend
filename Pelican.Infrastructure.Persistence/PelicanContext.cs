using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence;

public class PelicanContext : DbContext, IPelicanContext
{
	public PelicanContext(DbContextOptions<PelicanContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PelicanContext).Assembly);
	}

	public override DbSet<TEntity> Set<TEntity>() where TEntity : class
	{
		return base.Set<TEntity>();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return base.SaveChangesAsync(cancellationToken);
	}
}

