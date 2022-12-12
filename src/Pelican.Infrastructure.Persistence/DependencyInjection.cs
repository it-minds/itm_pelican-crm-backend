using System.Runtime.CompilerServices;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application;
using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence.DataLoader;
using Pelican.Infrastructure.Persistence.Interceptors;
using Pelican.Infrastructure.Persistence.Repositories;

[assembly: InternalsVisibleTo("Pelican.Infrastructure.Persistence.Test")]
namespace Pelican.Infrastructure.Persistence;

public static class DependencyInjection
{
	//This depedency injection allows persistence to be added as a service in program.
	public static IServiceCollection AddPersistince(
		this IServiceCollection services,
		IConfiguration configuration,
		bool isProduction)
	{
		services.AddSingleton<UpdateTimeTrackedEntitiesInterceptor>();

		string connectionString = isProduction
			? GetDevConnectionString(configuration)
			: AddLocalDb(configuration);

		services.AddDbContextFactory<PelicanContext>((serviceProvider, dbContextOptionsBuilder) =>
		{
			var timetrackedInterceptor = serviceProvider.GetService<UpdateTimeTrackedEntitiesInterceptor>()
				?? throw new NullReferenceException($"{typeof(UpdateTimeTrackedEntitiesInterceptor)} is null");

			dbContextOptionsBuilder
				.UseSqlServer(
					connectionString,
					SqlServerDbContextOptionsBuilder => SqlServerDbContextOptionsBuilder
						.MigrationsAssembly(typeof(PelicanContext).Assembly.FullName))
				.AddInterceptors(timetrackedInterceptor!);
		});

		services.AddTransient<IUnitOfWork>(
			serviceProvider => new UnitOfWork(
				serviceProvider
					.GetRequiredService<IDbContextFactory<PelicanContext>>()
					.CreateDbContext()));

		services.AddTransient<IPelicanBogusFaker, PelicanBogusFaker>();

		services.AddTransient<DevelopmentSeeder>();

		return services;
	}
	public static IRequestExecutorBuilder AddDataLoaders(this IRequestExecutorBuilder builder)
	{
		builder
			.AddDataLoader<IGenericDataLoader<AccountManager>, GenericDataLoader<AccountManager>>()
			.AddDataLoader<IGenericDataLoader<Client>, GenericDataLoader<Client>>()
			.AddDataLoader<IGenericDataLoader<Contact>, GenericDataLoader<Contact>>()
			.AddDataLoader<IGenericDataLoader<Deal>, GenericDataLoader<Deal>>()
			.AddDataLoader<IGenericDataLoader<Supplier>, GenericDataLoader<Supplier>>();

		return builder;
	}

	public static WebApplication UsePersistence(this WebApplication app)
	{
		////Readd the below lines later when frontend is done testing with seeded data
		//if (app.Environment.IsDevelopment())
		//{
		//app
		//	.Services
		//	.GetRequiredService<DevelopmentSeeder>()
		//	.SeedEntireDb(20);
		//}

		return app;
	}

	private static string AddLocalDb(IConfiguration configuration)
		=> configuration.GetConnectionString("myLocalDb");

	private static string GetDevConnectionString(IConfiguration configuration)
	{
		string keyVaultName = configuration["KeyVaultName"];

		var kvUri = $"https://{keyVaultName}.vault.azure.net";

		var client = new SecretClient(
			new Uri(kvUri),
			new DefaultAzureCredential());

		return client
			.GetSecret(configuration["PelicanMsSQLSecret"])
			.Value
			.Value;
	}
}
