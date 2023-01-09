using Pelican.Application;
using Pelican.Domain;
using Pelican.Infrastructure.HubSpot;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.Api;
using Pelican.Presentation.GraphQL;

const string CORS_PROD = "CorsProd";
const string ALLOW_ALL = "AllowAll";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDomain(builder.Configuration, builder.Environment.IsProduction());
builder.Services.AddPersistince(builder.Configuration, builder.Environment.IsProduction());
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddHubSpot();


builder.Services.AddCors(options =>
{
	options
		.AddPolicy(
			CORS_PROD,
			policy => policy
				.WithOrigins(
					"https://localhost",
					"http://localhost",
					"https://proud-sea-01fd94a03.2.azurestaticapps.net")
				.AllowAnyHeader()
				.AllowAnyMethod());
	options
		.AddPolicy(
			ALLOW_ALL,
			policy => policy
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());
});

builder.Services.AddApi();
builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

if (app.Environment.IsProduction())
{
	app.UseCors(CORS_PROD);
}
else
{
	app.UseCors(ALLOW_ALL);
}

app.UseApi();
app.UseGraphQL();
app.UsePersistence();

app.Run();
