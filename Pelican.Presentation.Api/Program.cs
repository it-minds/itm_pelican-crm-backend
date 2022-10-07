using Pelican.Application;
using Pelican.Infrastructure.HubSpot;
using Pelican.Infrastructure.Persistence;
using Pelican.Presentation.Api;

const string allowedCorsOrigins = "AllowedCorsOrigins";

var builder = WebApplication.CreateBuilder(args);

builder
	.Services
	.AddCors(options => options
		.AddPolicy(name: allowedCorsOrigins, policy => policy
			.WithOrigins("https://localhost")));

builder.Services.AddHubSpot(builder.Configuration);
builder.Services.AddPersistince(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi();

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
