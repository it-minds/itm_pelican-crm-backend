using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Primitives;

namespace Pelican.Application.Abstractions.Data;

public interface IPelicanContext : IDisposable
{
	DbSet<T> Set<T>() where T : class;

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
