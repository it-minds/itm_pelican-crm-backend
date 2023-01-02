using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

public sealed record NewInstallationHubSpotCommand(
	string Code) : ICommand;
