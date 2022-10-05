using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand>
{
	private readonly IHubSpotAuthorizationService _hubSpotService;

	public NewInstallationCommandHandler(IHubSpotAuthorizationService hubSpotService)
	{
		_hubSpotService = hubSpotService;
	}

	public async Task<Result> Handle(NewInstallationCommand command, CancellationToken cancellationToken)
	{
		Result result = await _hubSpotService.AuthorizeUserAsync(command.Code, cancellationToken);
		return result;
	}
}
