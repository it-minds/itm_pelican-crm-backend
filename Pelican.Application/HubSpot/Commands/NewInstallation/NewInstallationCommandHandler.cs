using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand>
{
	private readonly IHubSpotAuthorizationService _hubSpotService;

	public NewInstallationCommandHandler(
		IHubSpotAuthorizationService hubSpotService)
		=> _hubSpotService = hubSpotService;

	public async Task<Result> Handle(
		NewInstallationCommand command,
		CancellationToken cancellationToken)
		=> await _hubSpotService.AuthorizeUserAsync(command.Code, cancellationToken);
}
