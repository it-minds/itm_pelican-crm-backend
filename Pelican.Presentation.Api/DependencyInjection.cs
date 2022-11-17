using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pelican.Presentation.Api.Mapping;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

[assembly: InternalsVisibleTo("Pelican.Presentation.Api.Test")]
namespace Pelican.Presentation.Api;

public static class DependencyInjection
{
	const string ALLOWED_CORS_ORIGINS = "AllowedCorsOrigins";

	public static IServiceCollection AddApi(this IServiceCollection services)
	{
		services.AddScoped<IHashGeneratorFactory, HashGeneratorFactory>();

		services.AddScoped<HubSpotValidationFilter>();

		services.AddScoped<IRequestToCommandMapper, WebHookRequestsToCommands>();

		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}

	public static WebApplication UseApi(this WebApplication app)
	{
		//Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		app.UseRouting();

		app.UseAuthorization();

		app.MapControllers();

		return app;
	}
}
