using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;

namespace Pelican.Infrastructure.Persistence;
public static class DependencyInjection
{
	public static IServiceCollection AddPersistince(this IServiceCollection services, IConfiguration configuration)
	{
		//services.AddDbContext<PelicanContext>(options => options.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
		//b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)), ServiceLifetime.Scoped);
		services.AddDbContextFactory<PelicanContext>(
			o => o.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
			b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)));
		services.AddTransient<IUnitOfWork>(_ => new UnitOfWork(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		return services;
	}
}
