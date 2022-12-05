using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Pelican.Application.Abstractions.Data;

public interface IPelicanContext : IDisposable
{
	DbSet<T> Set<T>() where T : class;

	EntityEntry<T> Entry<T>(T entity) where T : class;

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
