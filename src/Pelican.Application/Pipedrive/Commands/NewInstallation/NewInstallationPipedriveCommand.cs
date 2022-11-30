using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Pipedrive.Commands.NewInstallation;

public sealed record NewInstallationPipedriveCommand(
	string Code) : ICommand;
