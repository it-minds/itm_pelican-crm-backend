using Pelican.Application;
using Pelican.Domain;
using Pelican.Infrastructure.HubSpot;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.Api;
using Pelican.Presentation.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddHubSpot();
builder.Services.AddPersistince(builder.Configuration, builder.Environment.IsProduction());
builder.Services.AddApplication();
builder.Services.AddApi();

//Adding GraphQl Specific extensions and services.
builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

app.UseApi();
app.UseGraphQL();
app.UsePersistence();

app.Run();
