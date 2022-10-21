using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Pelican.Presentation.Api.Test")]
namespace Pelican.Presentation.Api;

public static class DependencyInjection
{
	public static IServiceCollection AddApi(this IServiceCollection services)
	{
		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}
}

