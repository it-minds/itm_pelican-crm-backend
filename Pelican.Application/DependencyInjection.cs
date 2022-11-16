using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		return services;
	}
}
