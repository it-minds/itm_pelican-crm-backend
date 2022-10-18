using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.DataLoader;
using Pelican.Infrastructure.Persistence.Repositories;
using Location = Pelican.Domain.Entities.Location;

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
		services.AddTransient<IPelicanBogusFaker, PelicanBogusFaker>();
		services.AddTransient<DevelopmentSeeder>();
		return services;
	}
	public static IRequestExecutorBuilder AddDataLoaders(this IRequestExecutorBuilder builder)
	{
		builder.AddDataLoader<IGenericDataLoader<AccountManager>, GenericDataLoader<AccountManager>>()
			.AddDataLoader<IGenericDataLoader<Client>, GenericDataLoader<Client>>()
			.AddDataLoader<IGenericDataLoader<Contact>, GenericDataLoader<Contact>>()
			.AddDataLoader<IGenericDataLoader<Deal>, GenericDataLoader<Deal>>()
			.AddDataLoader<IGenericDataLoader<Location>, GenericDataLoader<Location>>()
			.AddDataLoader<IGenericDataLoader<Supplier>, GenericDataLoader<Supplier>>();
		return builder;
	}
}
