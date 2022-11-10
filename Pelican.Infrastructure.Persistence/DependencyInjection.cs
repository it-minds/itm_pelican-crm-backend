using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pelican.Application;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence.DataLoader;
using Pelican.Infrastructure.Persistence.Repositories;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Infrastructure.Persistence;
public static class DependencyInjection
{
	//This depedency injection allows persistence to be added as a service in program.
	public static IServiceCollection AddPersistince(this IServiceCollection services, IConfiguration configuration, bool isProduction)
	{
		//if (isProduction)
		//{
		AddDevDb(services, configuration);
		//}
		//else
		//{
		//AddLocalDb(services, configuration);
		//}
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

	public static WebApplication UsePersistence(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.Services.GetRequiredService<DevelopmentSeeder>().SeedEntireDb(10);
		}

		return app;
	}
	private static IServiceCollection AddLocalDb(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextFactory<PelicanContext>(
			o => o.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
			b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)));
		return services;
	}
	private static IServiceCollection AddDevDb(this IServiceCollection services, IConfiguration configuration)
	{
		string keyVaultName = configuration["KeyVaultName"];
		var kvUri = "https://" + keyVaultName + ".vault.azure.net";

		var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
		Console.WriteLine(client.GetSecret(configuration["PelicanMsSQLSecret"]).Value.Value);
		services.AddDbContextFactory<PelicanContext>(
			o => o.UseSqlServer(client.GetSecret(configuration["PelicanMsSQLSecret"]).Value.Value,
			b => b.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName)));
		return services;
	}
}
