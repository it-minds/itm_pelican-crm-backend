﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;

namespace Pelican.Infrastructure.Persistence;
public static class DependencyInjection
{
	public static IServiceCollection AddPersistince(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<IPelicanContext, IDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("myLocalDb"),
			b => b.MigrationsAssembly(typeof(IDbContext).Assembly.FullName)), ServiceLifetime.Scoped);
		services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
		services.AddDbContextFactory<IDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("myLocalDb")), ServiceLifetime.Scoped);
		return services;
	}
}
