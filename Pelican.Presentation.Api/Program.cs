using Microsoft.EntityFrameworkCore;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.Api.Extension;
using Pelican.Presentation.GraphQL.AccountManagers;
using Pelican.Presentation.GraphQL.Clients;
using Pelican.Presentation.GraphQL.Contacts;
using Pelican.Presentation.GraphQL.DataLoader;
using Pelican.Presentation.GraphQL.Deals;
using Pelican.Presentation.GraphQL.Locations;
using Pelican.Presentation.GraphQL.Suppliers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureRepositoryWrapper();
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddDbContextFactory<PelicanContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("myLocalDb")));
//builder.Services.AddPooledDbContextFactory<PelicanContext>(options =>
//{
//	//options.UseNpgsql(System.Environment.GetEnvironmentVariable("DATABASE_URL"),
//	//    builder => builder
//	//        .MigrationsAssembly(typeof(SursenContext).Assembly.FullName)
//	//        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));

//	//options.UseNpgsql(DatabaseUrl,
//	//	builder => builder
//	//		.MigrationsAssembly(typeof(SursenContext).Assembly.FullName)
//	//		.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
//});

//builder.Services.AddScoped<PelicanContext>(provider =>
//{
//	var factory = provider.GetRequiredService<IDbContextFactory<PelicanContext>>();
//	return factory.CreateDbContext();
//});

builder.Services.AddGraphQLServer()
			.AddQueryType(q => q.Name("Query"))
			.AddTypeExtension<AccountManagersQuery>()
			.AddTypeExtension<ClientsQuery>()
			.AddTypeExtension<ContactsQuery>()
			.AddTypeExtension<DealsQuery>()
			.AddTypeExtension<LocationsQuery>()
			.AddTypeExtension<SuppliersQuery>()
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
			.AddFiltering()
			.AddSorting()
.AddDataLoader<AccountManagerByIdDataLoader>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var pelicanContext = scope.ServiceProvider.GetRequiredService<PelicanContext>();


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
