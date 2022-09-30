using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pelican.Infrastructure.Persistence;
public static class DependencyInjection
{
	public static IServiceCollection AddPersistince(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<PelicanContext>(options => options.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
			b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)), ServiceLifetime.Transient);
		return services;
	}
}
