using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;

namespace Pelican.Infrastructure.Persistence;
public static class DependencyInjection
{
	//This depedency injection allows persistence to be added as a service in program.
	public static IServiceCollection AddPersistince(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextFactory<PelicanContext>(
			o => o.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
			b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)));
		services.AddTransient<IUnitOfWork>(_ => new UnitOfWork(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		return services;
	}
}
