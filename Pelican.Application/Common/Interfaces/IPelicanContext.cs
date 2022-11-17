using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;


namespace Pelican.Application.Common.Interfaces;

public interface IPelicanContext : IDisposable
{
	int SaveChanges();
	int SaveChanges(bool acceptAllChangesOnSuccess);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
}
