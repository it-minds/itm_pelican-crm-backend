namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(Assembly.GetExecutingAssembly());
		return services;
	}
}
