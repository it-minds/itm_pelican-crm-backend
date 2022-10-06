using HotChocolate;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL.AccountManagers;
using Pelican.Presentation.GraphQL.Clients;
using Pelican.Presentation.GraphQL.Contacts;
using Pelican.Presentation.GraphQL.DataLoader;
using Pelican.Presentation.GraphQL.Deals;
using Pelican.Presentation.GraphQL.Errors;
using Pelican.Presentation.GraphQL.Locations;
using Pelican.Presentation.GraphQL.Suppliers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddPersistince(builder.Configuration);

var executorBuilder = builder.Services.AddGraphQLServer()
	.AddQueryType(q => q.Name("Query"))
			.AddTypeExtension<AccountManagersQuery>()
			.AddTypeExtension<ClientsQuery>()
			.AddTypeExtension<ContactsQuery>()
			.AddTypeExtension<DealsQuery>()
			.AddTypeExtension<LocationsQuery>()
			.AddTypeExtension<SuppliersQuery>()
			.AddProjections()
			.AddFiltering()
			.AddSorting()
			.AddDataLoader<AccountManagerByIdDataLoader>()
			.AddDataLoader<ClientByIdDataLoader>()
			.AddDataLoader<ContactByIdDataLoader>()
			.AddDataLoader<DealByIdDataLoader>()
			.AddDataLoader<LocationByIdDataLoader>()
			.AddDataLoader<SupplierByIdDataLoader>()
			.AddErrorFilter<PelicanErrorFilter>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var pelicanContext = scope.ServiceProvider.GetRequiredService<IDbContext>();


	if (!pelicanContext.Suppliers.Any())
	{
		DevelopmentSeeder.SeedEntireDb(pelicanContext);
	}
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGraphQL();
});



app.Run();


