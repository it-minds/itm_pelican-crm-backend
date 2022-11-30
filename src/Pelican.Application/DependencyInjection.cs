﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.Behaviours;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddSingleton<IClient, RestSharpClient>();

		services.AddMediatR(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		return services;
	}
}
