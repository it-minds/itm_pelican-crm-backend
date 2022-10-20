﻿using Pelican.Application;
using Pelican.Infrastructure.HubSpot;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.Api;
using Pelican.Infrastructure.Authentication;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.GraphQL;

const string allowedCorsOrigins = "AllowedCorsOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
	.Services
	.AddCors(options => options
		.AddPolicy(name: allowedCorsOrigins, policy => policy
			.WithOrigins("https://localhost")));

builder.Services.AddHubSpot(builder.Configuration);
builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi();

//builder.Services.AddAzure();
builder.Services.AddAuth(builder.Configuration);

//Adding GraphQl Specific extensions and services.
builder.Services
	.AddPresentationGraphQL()
	.AddDataLoaders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseCookiePolicy();
app.UseAuthorization();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGraphQL();
	endpoints.MapControllers();

});

app.UseCors(allowedCorsOrigins);

app.MapControllers();
app.Services.GetRequiredService<DevelopmentSeeder>().SeedEntireDb(10);

app.Run();




