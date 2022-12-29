using System.Reflection;
using System.Runtime.CompilerServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.Behaviours;
using Pelican.Application.Options;
using Pelican.Application.RestSharp;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Application.Security;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<ICurrentUserService, CurrentUserService>();

		services.AddSingleton<IClient<HubSpotSettings>, RestSharpClient<HubSpotSettings>>();

		services.AddMediatR(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.Tokens));

		return services;
	}
}
