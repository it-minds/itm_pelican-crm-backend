﻿using MediatR;
using Pelican.Infrastructure.HubSpot;

var allowedCorsOrigins = "AllowedCorsOrigins";
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHubSpot(builder.Configuration);

builder
	.Services
	.AddCors(options => options
		.AddPolicy(name: allowedCorsOrigins, policy => policy
			.WithOrigins("https://localhost")));


//builder
//	.Services
//	.Scan(scan => scan
//		.FromAssemblies(Pelican.Infrastructure.HubSpot.AssemblyReference.Assembly)
//		.AddClasses(false)
//		.AsImplementedInterfaces()
//		.WithScopedLifetime());



builder.Services.AddMediatR(Pelican.Application.AssemblyReference.Assembly);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowedCorsOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
