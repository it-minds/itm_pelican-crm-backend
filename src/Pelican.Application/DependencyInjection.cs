﻿using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.Behaviours;
using Pelican.Application.Options;
using Pelican.Application.RestSharp;
using Pelican.Application.Security;
using Pelican.Domain.Settings.HubSpot;


[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services.AddScoped<ICurrentUserService, CurrentUserService>();

		services.AddScoped<IAuthorizationService, AuthorizationService>();

		services.AddScoped<IPasswordHasher, PasswordHasher>();

		services.AddSingleton<IClient<HubSpotSettings>, RestSharpClient<HubSpotSettings>>();

		services.AddMediatR(Assembly.GetExecutingAssembly());

		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped<IGetCustomAttributesService, GetCustomAttributesService>();

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.Tokens));

		services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

		services.AddScoped<ITokenService, TokenService>();

		return services;
	}
}
