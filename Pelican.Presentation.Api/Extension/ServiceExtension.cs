using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;

namespace Pelican.Presentation.Api.Extension;

public static class ServiceExtension
{
	public static void ConfigureRepositoryWrapper(this IServiceCollection services)
	{
		services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
	}
}
