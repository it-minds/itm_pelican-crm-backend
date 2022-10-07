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
		services.AddTransient<ISupplierRepository>(_ => new SupplierRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		services.AddTransient<IAccountManagerRepository>(_ => new AccountManagerRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		services.AddTransient<ILocationRepository>(_ => new LocationRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		services.AddTransient<IContactRepository>(_ => new ContactRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		services.AddTransient<IClientRepository>(_ => new ClientRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));

		services.AddTransient<IDealRepository>(_ => new DealRepository(_.GetRequiredService<IDbContextFactory<PelicanContext>>().CreateDbContext()));
		return services;
	}
}
