using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Presentation.GraphQL.AccountManagers;
using Pelican.Presentation.GraphQL.DataLoader;

namespace Pelican.Presentation.GraphQL;
public static class DepedencyInjection
{
	public static IRequestExecutorBuilder AddQueryAndServer(this IRequestExecutorBuilder services)
	{
		services.AddGraphQLServer()
			//.AddQueryType(q => q.Name("Query"))
			.AddTypeExtension<AccountManagersQuery>()
			//.AddTypeExtension<ClientsQuery>()
			//.AddTypeExtension<ContactsQuery>()
			//.AddTypeExtension<DealsQuery>()
			//.AddTypeExtension<LocationsQuery>()
			//.AddTypeExtension<SuppliersQuery>()
			//.AddMutationType(m => m.Name("Mutation"))
			//.AddTypeExtension<ClientContactsMutation>()
			//.AddTypeExtension<AccountManagerDealsMutation>()
			//.AddTypeExtension<AccountManagersMutation>()
			//.AddTypeExtension<ClientsMutation>()
			//.AddTypeExtension<ContactsMutation>()
			//.AddTypeExtension<DealsMutation>()
			//.AddTypeExtension<DealContactsMutation>()
			//.AddTypeExtension<LocationsMutation>()
			//.AddTypeExtension<SuppliersMutation>()
			.AddProjections()
			.ConfigureSchema(s => s.ModifyOptions(opts => opts.StrictValidation = false))
			.AddFiltering()
			.AddSorting()
			.AddDataLoader<AccountManagerByIdDataLoader>();
		//.AddDataLoader<ClientByIdDataLoader>()
		//.AddDataLoader<ContactByIdDataLoader>()
		//.AddDataLoader<DealByIdDataLoader>()
		//.AddDataLoader<LocationByIdDataLoader>()
		//.AddDataLoader<SupplierByIdDataLoader>();
		return services;
	}
}
