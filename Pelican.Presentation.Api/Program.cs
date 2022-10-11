using Pelican.Application;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddApplication();


//Adding GraphQl Specific extensions and services.
var executorBuilder = builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGraphQL();
});



app.Run();


