using System.Runtime.CompilerServices;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

[assembly: InternalsVisibleTo("Pelican.Presentation.Api.Test")]
namespace Pelican.Presentation.Api;

public static class DependencyInjection
{
	public static IServiceCollection AddApi(this IServiceCollection services)
	{
		services.AddScoped<IHashGeneratorFactory, HashGeneratorFactory>();

		services.AddScoped<HubSpotValidationFilter>();

		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}
}

