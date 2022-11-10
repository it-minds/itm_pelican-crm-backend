using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

[assembly: InternalsVisibleTo("Pelican.Presentation.Api.Test")]
namespace Pelican.Presentation.Api;

public static class DependencyInjection
{
	const string ALLOWED_CORS_ORIGINS = "AllowedCorsOrigins";

	public static IServiceCollection AddApi(this IServiceCollection services)
	{
		services.AddCors(options => options
			.AddPolicy(name: ALLOWED_CORS_ORIGINS, policy => policy
				.WithOrigins("https://localhost")));

		services.AddScoped<IHashGeneratorFactory, HashGeneratorFactory>();

		services.AddScoped<HubSpotValidationFilter>();

		services.AddControllers();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		return services;
	}

	public static WebApplication UseApi(this WebApplication app)
	{
		// Configure the HTTP request pipeline.
		//if (app.Environment.IsDevelopment())
		//{
		app.UseSwagger();
		app.UseSwaggerUI();

		//}

		app.UseHttpsRedirection();
		app.UseRouting();

		app.UseAuthorization();

		app.UseCors(ALLOWED_CORS_ORIGINS);

		app.MapControllers();

		return app;
	}
}
