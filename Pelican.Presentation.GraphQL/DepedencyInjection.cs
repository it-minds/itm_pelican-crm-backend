using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelican.Presentation.GraphQL.AccountManager;
using Pelican.Presentation.GraphQL.AccountManagerDeals;
using Pelican.Presentation.GraphQL.AccountManagers;
using Pelican.Presentation.GraphQL.ClientContacts;
using Pelican.Presentation.GraphQL.Clients;
using Pelican.Presentation.GraphQL.Contacts;
using Pelican.Presentation.GraphQL.DataLoader;
using Pelican.Presentation.GraphQL.DealContacts;
using Pelican.Presentation.GraphQL.Deals;
using Pelican.Presentation.GraphQL.Locations;
using Pelican.Presentation.GraphQL.Suppliers;

namespace Pelican.Presentation.GraphQL;
public static class DepedencyInjection
{
	public static IRequestExecutorBuilder AddGraphQlServer(this IRequestExecutorBuilder services)
	{
		services.AddGraphQLServer()
			.AddQueryType(q => q.Name("Query"))
			.AddTypeExtension<ClientContactsQuery>()
			.AddTypeExtension<AccountMangerDealsQuery>()
			.AddTypeExtension<AccountManagersQuery>()
			.AddTypeExtension<ClientsQuery>()
			.AddTypeExtension<ContactsQuery>()
			.AddTypeExtension<DealsQuery>()
			.AddTypeExtension<DealContactsQuery>()
			.AddTypeExtension<LocationsQuery>()
			.AddTypeExtension<SuppliersQuery>()
			.AddMutationType(m => m.Name("Mutation"))
			.AddTypeExtension<ClientContactsMutation>()
			.AddTypeExtension<AccountManagerDealsMutation>()
			.AddTypeExtension<AccountManagersMutation>()
			.AddTypeExtension<ClientsMutation>()
			.AddTypeExtension<ContactsMutation>()
			.AddTypeExtension<DealsMutation>()
			.AddTypeExtension<DealContactsMutation>()
			.AddTypeExtension<LocationsMutation>()
			.AddTypeExtension<SuppliersMutation>()
			.AddProjections()
			.AddFiltering()
			.AddSorting()
			.AddDataLoader<ClientContactsByIdDataLoader>()
			.AddDataLoader<ClientByIdDataLoader>();
		return services;
	}
}
