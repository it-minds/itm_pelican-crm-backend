using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.Pipedrive;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Pipedrive.Services;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Pelican.Infrastructure.Pipedrive.Test")]
namespace Pelican.Infrastructure.HubSpot;

public static class DependencyInjection
{
	public static IServiceCollection AddHubSpot(
		this IServiceCollection services)
	{
		services.AddSingleton<IPipedriveService<Deal>, PipedriveDealService>();

		return services;
	}
}
