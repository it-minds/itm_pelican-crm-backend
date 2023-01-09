using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.CompilerServices;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.Behaviours;
using Pelican.Application.Mail;
using Pelican.Application.Options;
using Pelican.Application.RazorEmails.Interfaces;
using Pelican.Application.RazorEmails.Services;
using Pelican.Application.RestSharp;
using Pelican.Application.Security;
using Pelican.Domain.Settings.HubSpot;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application;

public static class DependencyInjection
{
	//Add application as a service that can be used in program
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		IConfiguration mailSettings;
		string keyVaultName = configuration["KeyVaultName"];
		var kvUri = "https://" + keyVaultName + ".vault.azure.net";
		var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

		mailSettings = configuration.GetRequiredSection("MailSettings");
		mailSettings["Password"] = client.GetSecret("PelicanSendgridSMTPRelayApiKey").Value.Value;

		services.AddHttpContextAccessor();

		services.AddScoped<ICurrentUserService, CurrentUserService>();

		services.AddScoped<IPasswordHasher, PasswordHasher>();

		services.AddScoped<IAuthorizationService, AuthorizationService>();

		services.AddScoped<IPasswordHasher, PasswordHasher>();

		services.AddScoped<IMailService, MailService>();

		services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

		services.AddSingleton<IClient<HubSpotSettings>, RestSharpClient<HubSpotSettings>>();

		services.AddMediatR(Assembly.GetExecutingAssembly());

		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped<IGetCustomAttributesService, GetCustomAttributesService>();

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.Tokens));

		services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

		services.AddScoped<ITokenService, TokenService>();

		return services;
	}
}
