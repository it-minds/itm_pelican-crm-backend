using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Presentation.GraphQL.AccountManagers;
using Pelican.Presentation.GraphQL.Clients;
using Pelican.Presentation.GraphQL.Contacts;
using Pelican.Presentation.GraphQL.Deals;
using Pelican.Presentation.GraphQL.Suppliers;

namespace Pelican.Presentation.GraphQL;
public static class DependencyInjection
{
	public static IRequestExecutorBuilder AddPresentationGraphQL(this IServiceCollection services)
	{
		return services.AddGraphQLServer()
			.AddQueryType(q => q.Name("Query"))
			.AddTypeExtension<AccountManagersQuery>()
			.AddTypeExtension<ClientsQuery>()
			.AddTypeExtension<ContactsQuery>()
			.AddTypeExtension<DealsQuery>()
			.AddTypeExtension<SuppliersQuery>()
			.AddProjections()
			.AddFiltering()
			.AddSorting();
	}

	public static WebApplication UseGraphQL(this WebApplication app)
	{
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapGraphQL();
		});

		return app;
	}
}
