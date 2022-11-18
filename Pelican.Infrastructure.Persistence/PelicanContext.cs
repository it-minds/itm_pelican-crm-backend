using Microsoft.EntityFrameworkCore;
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
}
