using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

public sealed record NewInstallationCommand(
	string Code,
	string BaseUrl,
	string ClientId,
	string RedirectUrl,
	string ClientSecret) : ICommand;
