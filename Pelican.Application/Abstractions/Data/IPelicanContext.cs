using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;


namespace Pelican.Application.Abstractions.Data;

public interface IPelicanContext : IDisposable
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
}
