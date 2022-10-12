using Pelican.Application;
using Pelican.Infrastructure.Google;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddGoogleAuth(builder.Configuration);


//Adding GraphQl Specific extensions and services.
var executorBuilder = builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseCookiePolicy();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGraphQL();
});



app.Run();


