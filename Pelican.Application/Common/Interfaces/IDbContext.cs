using Microsoft.EntityFrameworkCore;

namespace Pelican.Application.Common.Interfaces;
public interface IDbContext : IDisposable
{
	DbContext Instance { get; }
}
